using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLitWebApp.Models;

namespace BLitWebApp.Data
{
    public class DbInitializer
    {
        //Initialize the database with sample data
        public static void Initialize(VehicleContext context)
        {
            context.Database.EnsureCreated();

            //If data exists already, do not initialize startup data
            if (context.Vehicles.Any()) { return; }

            //Initialize Manufacturer names for the database
            var manufacturers = new Manufacturer[]
            {
                new Manufacturer{Name="Ford"},
                new Manufacturer{Name="Honda"},
                new Manufacturer{Name="Toyota"},
                new Manufacturer{Name="Volkswagon"}
            };
            foreach (Manufacturer m in manufacturers)
            {
                context.Manufacturers.Add(m);
            }
            context.SaveChanges();

            //Initialize CarModel names for the database
            var carModels = new CarModel[]
            {
                new CarModel{Name="F150",ManufacturerID=1},
                new CarModel{Name="Accord",ManufacturerID=2},
                new CarModel{Name="Camry",ManufacturerID=3},
                new CarModel{Name="Beetle",ManufacturerID=4}
            };
            foreach (CarModel c in carModels)
            {
                context.CarModels.Add(c);
            }
            context.SaveChanges();

            //Initialize CarClass names for the database
            var carClasses = new CarClass[]
            {
                new CarClass{Name="Compact"},
                new CarClass{Name="Pickup Truck"},
                new CarClass{Name="Subcompact"}
            };
            foreach (CarClass c in carClasses)
            {
                context.CarClasses.Add(c);
            }
            context.SaveChanges();

            //Initialize Vehicle names for the database
            var vehicles = new Vehicle[]
            {
                new Vehicle{VIN="11112222333344445", CarModelID=1, CarClassID=2},
                new Vehicle{VIN="11112222333377775", CarModelID=2, CarClassID=1},
                new Vehicle{VIN="88882222333344445", CarModelID=3, CarClassID=1},
                new Vehicle{VIN="11119999333344445", CarModelID=4, CarClassID=3}
            };
            foreach (Vehicle v in vehicles)
            {
                context.Vehicles.Add(v);
            }
            context.SaveChanges();
        }
    }
}
