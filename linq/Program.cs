using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

class Wczytywacz
{
    public List<T> WczytajListe<T>(string path, Func<string[], T> generuj)
    {
        var lines = File.ReadAllLines(path);
        var lista = new List<T>();

        foreach (var line in lines.Skip(1)) 
        {
            var fields = line.Split(',');
            lista.Add(generuj(fields));
        }

        return lista;
    }
}
class Region{
    public string regionid{get;set;}
    public string regiondescription {get;set;}

    public Region(string regionID, string regionDescription){
        regionid=regionID;
        regiondescription=regionDescription;
    }
}
class Territory{
    public string territoryid {get;set;}
    public string territorydescription {get;set;}
    public string regionid {get;set;}

    public Territory(string territoryid_,string territorydescription_,string regionid_){
        territoryid=territoryid_;
        territorydescription=territorydescription_;
        regionid=regionid_;
    }
}

class EmplyeeTerritory{
    public string employeeid {get;set;}
    public string territoryid {get;set;}
    public EmplyeeTerritory(string employeeid_,string territoryid_ ){
        employeeid=employeeid_;
        territoryid=territoryid_;
    }
}

class Employee{
    public string employeeid {get;set;}
    public string lastname {get;set;}
    public string firstname {get;set;}
    public string title {get;set;}
    public string titleofcourtesy {get;set;}
    public string birthdate {get;set;}
    public string hiredate {get;set;}
    public string address {get;set;}
    public string city {get;set;}
    public string region {get;set;}
    public string postalcode {get;set;}
    public string country {get;set;}
    public string homephone {get;set;}
    public string extension {get;set;}
    public string photo {get;set;}
    public string notes {get;set;}
    public string reportsto {get;set;}
    public string photopath {get;set;}
    public Employee(string employeeid_,string lastname_,string firstname_, string title_,string titleofcourtesy_,
    string birthdate_, string hiredate_, string address_,string city_,string region_,string postalcode_,
    string country_,string homephone_,string extension_,string photo_,string notes_,string reportsto_,string photopath_){

        employeeid=employeeid_;
        lastname=lastname_;
        firstname=firstname_;
        title=title_;
        titleofcourtesy=titleofcourtesy_;
        birthdate=birthdate_;
        hiredate=hiredate_;
        address=address_;
        city=city_;
        region=region_;
        postalcode=postalcode_;
        country=country_;
        homephone=country_;
        extension=extension_;
        photo=photo_;
        notes=notes_;
        reportsto=reportsto_;
        photopath=photopath_;
}}

class Order{
    public string orderid {get;set;}
    public string customerid {get;set;}
    public string employeeid {get;set;}
    public string orderdate {get;set;}
    public string requireddate {get;set;}
    public string shippeddate {get;set;}
    public string shipvia {get;set;}
    public string freight {get;set;}
    public string shipname {get;set;}
    public string shipaddress {get;set;}
    public string shipcity {get;set;}
    public string shipregion {get;set;}
    public string shippostalcode {get;set;}
    public string shipcountry {get;set;}
    public Order(string orderid_,string customerid_, string employeeid_,string orderdate_, string requireddate_,string shippeddate_,
    string shipvia_,string freight_,string shipname_,string shipaddress_,string shipcity_,string shipregion_,string shippostalcode_,string shipcountry_){
        orderid=orderid_;
        customerid=customerid_;
        employeeid=employeeid_;
        orderdate=orderdate_;
        requireddate=requireddate_;
        shippeddate=shippeddate_;
        shipvia=shipvia_;
        freight=freight_;
        shipname=shipname_;
        shipaddress=shipaddress_;
        shipcity=shipcity_;
        shipregion=shipregion_;
        shippostalcode=shippostalcode_;
        shipcountry=shipcountry_;
    }
}
class OrderDetail{
    public string orderid {get;set;}
    public string productid {get;set;}
    public string unitprice {get;set;}
    public string  quantity {get;set;}
    public string discount {get;set;}
    public OrderDetail(string orderid_,string productid_,string unitprice_,string quantity_,string discount_){
        orderid=orderid_;
        productid=productid_;
        unitprice=unitprice_;
        quantity=quantity_;
        discount=discount_;
    }
}
class Program{

    static void Main(string[] args)
    {
        var loader = new Wczytywacz();

        List<Region> regions = loader.WczytajListe("import-northwind-dataset/regions.csv",x=> new Region(x[0],x[1]));
        List<Territory> territories = loader.WczytajListe("import-northwind-dataset/territories.csv",x=> new Territory(x[0],x[1],x[2]));
        List<EmplyeeTerritory> emplyee_territories = loader.WczytajListe("import-northwind-dataset/employee_territories.csv", x => new EmplyeeTerritory(x[0], x[1]));
        List<Employee> employees = loader.WczytajListe("import-northwind-dataset/employees.csv", x => new Employee(x[0], x[1],x[2],x[3],x[4],x[5],x[6],x[7],x[8],x[9],x[10],x[11],x[12],x[13],x[14],x[15],x[16],x[17]));
        List<Order> orders = loader.WczytajListe("import-northwind-dataset/orders.csv", x => new Order(x[0], x[1],x[2],x[3],x[4],x[5],x[6],x[7],x[8],x[9],x[10],x[11],x[12],x[13]));
        List<OrderDetail> order_detail = loader.WczytajListe("import-northwind-dataset/orders_details.csv",x=>new OrderDetail(x[0],x[1],x[2],x[3],x[4]));

        Console.WriteLine("Regions:");
        foreach(var region in regions){
            Console.WriteLine("ID: "+ region.regionid+ " Describtion: "+ region.regiondescription);
        }
        int i=0;
        Console.WriteLine("Territory:");
        foreach(var teritory in territories){
            if (i<5){
            Console.WriteLine("ID: "+teritory.regionid+" DESCRIPTION: "+teritory.territorydescription+" REGION: "+teritory.regionid);
            i++;
        }
            else {break;}
            }

        int i2=0;
        Console.WriteLine("EMPLOYEE TERRITORY:");
        foreach (var employeeteritory in emplyee_territories){
            if (i2<5){
                Console.WriteLine("EMPLOYEE ID: "+ employeeteritory.employeeid+ " TERRITORY ID: "+employeeteritory.territoryid);
                i2++;
            }
            else{break;}
        }


        //Wypiswanie nazwisk
        Console.WriteLine("___________________________");
        Console.WriteLine("WYPISYWANIE NAZWISK:");
        var nazwiskoQuery= from employee in employees select employee.lastname;
        foreach (var nazwisko in nazwiskoQuery){
            Console.WriteLine(nazwisko);
        }
        // Wypisz nazwiska pracownikow oraz dla kazego z nich nazwe regionu i terytorium gdzie pracuje
        Console.WriteLine("___________________________");
        Console.WriteLine("WYPISYWANIE NAZWISK,REGIONU I TERYTORIUM PRACY");
        var daneQuery = from emplyee in employees 
                        join employeeTerritory in emplyee_territories on emplyee.employeeid equals employeeTerritory.employeeid
                        join territory in territories on employeeTerritory.territoryid equals territory.territoryid
                        join region in regions on territory.regionid equals region.regionid
                        select new
                        {   
                            Nazwisko= emplyee.lastname,
                            Region = region.regiondescription,
                            Terytorium = territory.territorydescription
                        };
         Console.WriteLine("Nazwiska pracowników oraz miejsce pracy:");
        foreach (var item in daneQuery)
        {
            Console.WriteLine($"Nazwisko: {item.Nazwisko}, Region: {item.Region}, Terytorium: {item.Terytorium}");
        }

        // Wypisz nazwy regionow oraz nazwiska pracownikow ktorzy pracuja w tych regionach
        Console.WriteLine("___________________________");
        Console.WriteLine("WYPISANIE NAZW REGIONOW I ICH PRACOWNIKOW:");
        var regionyQuery = from employee in employees
            join employeeTerritory in emplyee_territories on employee.employeeid equals employeeTerritory.employeeid
            join territory in territories on employeeTerritory.territoryid equals territory.territoryid
            join region in regions on territory.regionid equals region.regionid
            group employee by region into regionGroup
            select new
            {
                RegionName = regionGroup.Key.regiondescription,
                Employees = regionGroup.Select(e => e.lastname).Distinct().ToList()
            };
        foreach (var region in regionyQuery)
        {
            Console.WriteLine($"Region: {region.RegionName}");
            foreach (var lastName in region.Employees)
            {
                Console.WriteLine($" - {lastName}");
            }

        }

        Console.WriteLine("___________________________");
        Console.WriteLine("WYPISANIE NAZW REGIONOW I LICZBY ICH PRACOWNIKOW:");
        var zliczQuery = from employee in employees
                        join employeeTerritory in emplyee_territories on employee.employeeid equals employeeTerritory.employeeid
                        join territory in territories on employeeTerritory.territoryid equals territory.territoryid
                        join region in regions on territory.regionid equals region.regionid
                        group employee by region into regionGroup
                        select new
                        {
                            RegionName = regionGroup.Key.regiondescription,
                            EmployeeCount = regionGroup.Count() // Liczymy liczbę pracowników w grupie dla danego regionu
                        };

        foreach (var region in zliczQuery)
        {
            Console.WriteLine($"Region: {region.RegionName} Liczba pracowników: {region.EmployeeCount}");
        }

        Console.WriteLine("___________________________");
        var ordersWithDetails = from order in orders
                                join detail in order_detail on order.orderid equals detail.orderid
                                select new {
                                    order.orderid,
                                    order.employeeid,
                                    TotalPrice = double.Parse(detail.unitprice.Replace(".", ",")) * int.Parse(detail.quantity.Replace(".", ","))
                                };


        var employeeStatistics = from order in ordersWithDetails
                                 group order by order.employeeid into employeeGroup
                                 select new {
                                     EmployeeID = employeeGroup.Key,
                                     OrderCount = employeeGroup.Count(),
                                     AverageOrderValue = employeeGroup.Average(o => o.TotalPrice),
                                     MaxOrderValue = employeeGroup.Max(o => o.TotalPrice)
                                 };


        Console.WriteLine("Statystyki zamówień:");
        foreach (var stat in employeeStatistics)
        {
            Console.WriteLine($"Pracownik: {stat.EmployeeID}");
            Console.WriteLine($"Liczba zamówień: {stat.OrderCount}");
            Console.WriteLine($"Średnia wartość zamówienia: {stat.AverageOrderValue:C}");
            Console.WriteLine($"Maksymalna wartość zamówienia: {stat.MaxOrderValue:C}");
            Console.WriteLine();
        }
    }
 }
