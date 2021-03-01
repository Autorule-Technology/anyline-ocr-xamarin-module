using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;



namespace AnylineExamples.Shared
{
    public enum ItemType
    {
        Item = 0,
        Header = 1,
        DocumentUI = 2
    }

    public static class Category
    {
        public static readonly string Vehicle = "Vehicle";
        public static readonly string Version = "Version";

        public static List<string> GetItems()
        {
            return new List<string>
            {
                Vehicle,
                Version
            };
        }
    }

    public static class Example
    {
        public static readonly string LicensePlate = "Scan VRN";
        public static readonly string ManualPlate = "Manual Entry";

    }

    public static class ExampleList
    {
        public static List<ExampleModel> Items { get; } = new List<ExampleModel>
        {
            new ExampleModel(ItemType.Header, Category.Vehicle, Category.Vehicle, ""),
            new ExampleModel(ItemType.Item, Example.LicensePlate, Category.Vehicle, "vehicle_config_license_plate.json"),
            new ExampleModel(ItemType.Item, Example.ManualPlate, Category.Vehicle, "vehicle_config_license_plate.json"),
            new ExampleModel(ItemType.Header, Category.Version, Category.Version, ""),
        };
    }
}

