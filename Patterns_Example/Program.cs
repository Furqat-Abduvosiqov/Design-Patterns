// Factory Method Example
// using Patterns_Example.Creational_Design_Patterns.Factory_Method;
//
// Console.WriteLine("=== Factory Method Pattern Demo ===\n");
//
// var logistics = new LogisticApplication[] 
// {
//     new RoadLogisitic(),
//     new SeaLogistic()
// };
//
// foreach (var logistic in logistics)
// {
//     logistic.PlanDelivery();
//     var transport = logistic.CreateTransport();
//     transport.Deliver();
//     Console.WriteLine($"Cost: ${transport.GetCost()}");
//     Console.WriteLine();
// }

// Abstract Factory Method Example

// using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.Abstractions;
// using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.ModernFurnitureFactory;
// using Patterns_Example.Creational_Design_Patterns.Abstract_Factory.VictorianFurnitureFactory;
//
// Console.WriteLine("=== Abstract Factory Method Pattern Demo ===\n");
//
// var factories = new FurnitureFactory[] 
// {
//     new VictorianFurnitureFactory(),
//     new ModernFurnitureFactory()
// };
//
// foreach (var factory in factories)
// {
//     var chair = factory.CreateChair();
//     var table = factory.CreateTable();
//     var sofa = factory.CreateSofa();
//     
//     Console.WriteLine($"Chair: {chair.GetName()} - ${chair.GetPrice()}");
//     Console.WriteLine($"Table: {table.GetName()} - ${table.GetPrice()}");
//     Console.WriteLine($"Sofa: {sofa.GetName()} - ${sofa.GetPrice()}");
//     Console.WriteLine();
// }

// Prototype Pattern Example
using Patterns_Example.Creational_Design_Patterns.Prototype;

PrototypeExample.RunExample();
