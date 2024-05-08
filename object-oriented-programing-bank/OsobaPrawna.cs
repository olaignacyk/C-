using System.ComponentModel;

public class OsobaPrawna : PosiadaczRachunku
{
    private string nazwa;
    private string siedziba;

     public OsobaPrawna(string nazwa_,string siedziba_){
        nazwa=nazwa_;
        siedziba=siedziba_;
        Console.WriteLine("Utworzone Osobe Prawna: "+ nazwa+" "+siedziba +" ");
    }

    public string Nazwa
    {
        get {return nazwa;}
        set {nazwa=value;}
    }

    public string Siedziba 
    {
        get {return siedziba;}
        set {siedziba=value;}
    }
    public override string ToString()
    {
        return "Osoba prawna: " + nazwa + " " + siedziba;
    }
}