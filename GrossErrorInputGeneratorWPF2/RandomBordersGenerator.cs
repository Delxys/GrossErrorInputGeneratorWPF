using GrossErrorInputGeneratorWPF2.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace GrossErrorInputGeneratorWPF2
{
    internal class RandomBordersGenerator : IGenerator
    {
        public void Generate(Settings settings)
        {
            var commonInputPath = "common-input-copyV3.json";
            var firstPath = "firstV3.json";
            var filesNumber = 200;
            var firstFlow = JsonConvert.DeserializeObject<Flow>(File.ReadAllText(firstPath));
            var balanceInput = JsonConvert.DeserializeObject<Flow[]>(File.ReadAllText(commonInputPath));
            int guidLastLength = balanceInput.First().Id.Split("-").Last().Length;
            string flowIdStart = "F-";
            string nodeIdStart = "N-";
            //int copies = 7;
            int comonInputFlows = 6;
            int basicInputFlows = 7;
            int size = settings.Multiplier * comonInputFlows + basicInputFlows;
            var errorsIdList = $"errorsIdListBordersS{size}.txt";
            File.Delete(errorsIdList);

            for (int num = 0; num < filesNumber; num++)
            {
                int numberOfNodes = 3;
                var newVaraibles = new List<Flow>(balanceInput);
                var generatedPath = ($"StringJsonBorders/generated-outputBorders{num}.json");
                var generatedDoublesPath = ($"DoublesJsonBorders/generated-outputDoublesBorders{num}.json");
                var random = new Random();
                var randomDouble = random.NextDouble();
                var randomMeasuredSum = Math.Round(randomDouble * (600 - 120) + 120, 2);
                var firstFlowMeasured = randomMeasuredSum;
                //подготовка input
                var currentSum = randomMeasuredSum;
                foreach (var balanceVariable in balanceInput)
                {
                    if (int.Parse(balanceVariable.Id.Split("-").Last()) % 2 == 1)
                    {
                        balanceVariable.Measured = currentSum;
                    }
                    else
                    {
                        currentSum -= balanceVariable.Measured;
                    }
                }
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                string serialziedInput = JsonConvert.SerializeObject(balanceInput, new JsonSerializerSettings() { ContractResolver = contractResolver, Formatting = Formatting.Indented });

                File.WriteAllText(commonInputPath, serialziedInput);
                Thread.Sleep(100);
                var randomedBalanceInput = JsonConvert.DeserializeObject<Flow[]>(File.ReadAllText(commonInputPath));


                for (int i = 0; i < settings.Multiplier; i++)
                {

                    var generatedVariables = randomedBalanceInput.Select(x =>
                    {
                        var randomDoubleForBorders = random.NextDouble();
                        var randomDoubleBorder = Math.Round(randomDoubleForBorders * (50 - 20) + 20, 2);
                        var newVariable = new Flow
                        {
                            Id = flowIdStart + (int.Parse(x.Id.Split("-").Last()) + newVaraibles.Count).ToString().PrependZero(guidLastLength - (int.Parse(x.Id.Split("-").Last()) + newVaraibles.Count).ToString().Length),
                            SourceId = x.SourceId is null ? null : nodeIdStart + (int.Parse(x.SourceId.Split("-").Last()) + numberOfNodes).ToString().PrependZero(guidLastLength - (int.Parse(x.SourceId.Split("-").Last()) + numberOfNodes).ToString().Length),
                            DestinationId = x.DestinationId is null ? null : nodeIdStart + (int.Parse(x.DestinationId.Split("-").Last()) + numberOfNodes).ToString().PrependZero(guidLastLength - (int.Parse(x.DestinationId.Split("-").Last()) + numberOfNodes).ToString().Length),
                            Name = $"X{int.Parse(x.Name.Split("X").Last()) + newVaraibles.Count}",
                            Measured = x.Measured,
                            UpperBound = x.Measured + randomDoubleBorder,
                            LowerBound = x.Measured > randomDoubleBorder ? x.Measured - randomDoubleBorder : 0.0,
                            Tolerance = x.Tolerance,
                            IsMeasured = x.IsMeasured,

                        };

                        if (int.Parse(newVariable.Id.Split("-").Last()) % 2 == 1)
                        {
                            newVariable.Measured = currentSum;
                            newVariable.UpperBound = x.Measured + randomDoubleBorder;
                            newVariable.LowerBound = x.Measured > randomDoubleBorder ? x.Measured - randomDoubleBorder : 0.0;
                            return newVariable;
                        }
                        currentSum -= newVariable.Measured;
                        return newVariable;
                    }).ToList();
                    numberOfNodes += 3;

                    newVaraibles.Last().DestinationId = nodeIdStart + (int.Parse(newVaraibles.Last().SourceId.Split("-").Last()) + 1).ToString().PrependZero(guidLastLength - (int.Parse(newVaraibles.Last().SourceId.Split("-").Last()) + 1).ToString().Length);
                    newVaraibles.AddRange(generatedVariables);
                }
                firstFlow.Measured = firstFlowMeasured;
                newVaraibles.Insert(0, firstFlow);
                //Генерация ошибки(-ок)
                int numberOfErrors = 1;
                var flowNum = 1;
                while (numberOfErrors > 0)
                {
                    var flowsCounter = newVaraibles.Count;
                    while (flowNum % 2 == 1)
                    {
                        flowNum = GuidExtensions.GetPseudoIntWithinRange(0, flowsCounter);
                    }
                    newVaraibles[flowNum].Measured = GuidExtensions.GetPseudoDoubleWithinRange(2000, 2300);
                    numberOfErrors--;
                }
                var errorIddata = num.ToString() + "    " + (flowNum + 1).ToString() + "\n";
                File.AppendAllTextAsync(errorsIdList, errorIddata);

                //добавление "пустых" потоков
                int maxCopiesNumber = 12;
                int emptySize = (maxCopiesNumber * comonInputFlows + basicInputFlows) - (settings.Multiplier * comonInputFlows + basicInputFlows);
                var emptyFlowList = new List<Flow>();

                for (int i = 0; i < emptySize; i++)
                {
                    var newVar = new Flow
                    {
                        Id = "F-000000000000",
                        SourceId = null,
                        DestinationId = null,
                        Name = $"X0",
                        Measured = 0.0,
                        UpperBound = 0.0,
                        LowerBound = 0.0,
                        Tolerance = 0.0,
                        IsMeasured = false,
                    };
                    emptyFlowList.Add(newVar);
                }
                newVaraibles.AddRange(emptyFlowList);

                //Переделал в Double
                List<FlowDouble> balanceDoubleVariables = new List<FlowDouble>();
                //1.__ - Id потока
                //2.__ - Id узла
                //3.__ - имя потока
                //0.0 - null
                for (int i = 0; i < newVaraibles.Count; i++)
                {
                    var newVariable = new FlowDouble();
                    balanceDoubleVariables.Add(newVariable);
                    balanceDoubleVariables[i].Id = newVaraibles[i].Id is "F-000000000000" ? 0.0 : Math.Round(1.0 + double.Parse(newVaraibles[i].Id.Split("-").Last()) / 100.0, 2);
                    balanceDoubleVariables[i].SourceId = newVaraibles[i].SourceId is null ? 0.0 : Math.Round(2.0 + double.Parse(newVaraibles[i].SourceId.Split("-").Last()) / 100.0, 2);
                    balanceDoubleVariables[i].DestinationId = newVaraibles[i].DestinationId is null ? 0.0 : Math.Round(2.0 + double.Parse(newVaraibles[i].DestinationId.Split("-").Last()) / 100.0, 2);
                    balanceDoubleVariables[i].Name = newVaraibles[i].Name is "X0" ? 0.0 : Math.Round(3.0 + double.Parse(newVaraibles[i].Name.Split("X").Last()) / 100.0, 2);
                    balanceDoubleVariables[i].Measured = newVaraibles[i].Measured;
                    balanceDoubleVariables[i].UpperBound = newVaraibles[i].UpperBound;
                    balanceDoubleVariables[i].LowerBound = newVaraibles[i].LowerBound;
                    balanceDoubleVariables[i].Tolerance = newVaraibles[i].Tolerance;
                    balanceDoubleVariables[i].IsMeasured = newVaraibles[i].IsMeasured;
                }


                SchemeDouble balanceDoubleInput = new SchemeDouble
                {
                    Flows = balanceDoubleVariables,
                    Label = flowNum 
                };
                string serialziedDoubleJson = JsonConvert.SerializeObject(balanceDoubleInput, new JsonSerializerSettings() { ContractResolver = contractResolver, Formatting = Formatting.Indented });

                File.WriteAllTextAsync(generatedDoublesPath, serialziedDoubleJson);

                Scheme balanceInput1 = new Scheme
                {
                    Flows = newVaraibles,
                    Label = $"X{flowNum + 1}",
                };
                string serialziedNormalJson = JsonConvert.SerializeObject(balanceInput1, new JsonSerializerSettings() { ContractResolver = contractResolver, Formatting = Formatting.Indented });

                File.WriteAllText(generatedPath, serialziedNormalJson);
            }
        }
    }
}
