// Utowrzenie osoby fizycznej i prawnej
OsobaFizyczna osoba = new OsobaFizyczna("Julia", "Kowalczyk", "Zuzanna", "00000000000", "abc123");
OsobaPrawna prawna = new OsobaPrawna("firma", "krakow");
OsobaFizyczna posiadacz1 = new OsobaFizyczna("Jan", "Kowalski", "Zuzanna", "00000000000", "abc123");
OsobaFizyczna posiadacz2 = new OsobaFizyczna("Anna", "Nowak", "Zuzanna", "00000000000", "abc123");

// Utworzenie rachunków 
RachunekBankowy rachunek1 = new RachunekBankowy("123456789", 1000, true, new List<PosiadaczRachunku> { posiadacz1 });
RachunekBankowy rachunek2 = new RachunekBankowy("987654321", 500, false, new List<PosiadaczRachunku> { posiadacz2 });

Console.WriteLine("_______________________________________");
Console.WriteLine(rachunek1);
Console.WriteLine("_______________________________________");
Console.WriteLine("\n");
Console.WriteLine("%%%%%%%%%%%%");
Console.WriteLine("_______________________________________");
Console.WriteLine(rachunek2);
Console.WriteLine("_______________________________________");
Console.WriteLine("\n");
Console.WriteLine("%%%%%%%%%%%%");

//Dodanie posiadaczy
rachunek1 += posiadacz2;
rachunek2 += prawna;


Console.WriteLine("_______________________________________");
Console.WriteLine(rachunek1);
Console.WriteLine("_______________________________________");
Console.WriteLine("\n");
Console.WriteLine("%%%%%%%%%%%%");

Console.WriteLine("TERAZ ILE JEST NA RACHUNKU nr1: "+rachunek1.StanRachunku);
Console.WriteLine("TERAZ ILE JEST NA RACHUNKU nr2: "+rachunek2.StanRachunku);
Console.WriteLine("%%%%%%%%%%%%"+ "\n");

// Dokonanie transakcji przelewu
RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 200, "Przelew");

Console.WriteLine("\n");
Console.WriteLine("%%%%%%%%%%%%");
Console.WriteLine("TERAZ ILE JEST NA RACHUNKU nr1: "+rachunek1.StanRachunku);
Console.WriteLine("TERAZ ILE JEST NA RACHUNKU nr2: "+rachunek2.StanRachunku);
Console.WriteLine("%%%%%%%%%%%%"+"\n");

Console.WriteLine("_______________________________________");
Console.WriteLine(rachunek1);
Console.WriteLine("_______________________________________");
Console.WriteLine(rachunek2);
Console.WriteLine("_______________________________________");

//Dokonanie wplaty
RachunekBankowy.DokonajTransakcji(null, rachunek1, 300, "Wpłata gotówkowa");

Console.WriteLine("\n");
Console.WriteLine("%%%%%%%%%%%%");
Console.WriteLine("TERAZ ILE JEST NA RACHUNKU nr1: "+rachunek1.StanRachunku);
Console.WriteLine("TERAZ ILE JEST NA RACHUNKU nr2: "+rachunek2.StanRachunku);
Console.WriteLine("%%%%%%%%%%%%"+"\n");

//Usuniecie posiadacza konta
rachunek1 -= posiadacz2;
Console.WriteLine("_______________________________________");
Console.WriteLine(rachunek1);
Console.WriteLine("_______________________________________");

// //Usuniecie obu wlascicieli
// rachunek1 -= posiadacz1;

// //Proba zrealizowania transakcji rzucającej wyjątek
// RachunekBankowy.DokonajTransakcji(null,null,400,"Test wyjątku");

//Test wyjatku z peselem
OsobaFizyczna test = new OsobaFizyczna("Olga","Kozioł","Anna","1234","abc123");