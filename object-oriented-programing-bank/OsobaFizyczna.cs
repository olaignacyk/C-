public class OsobaFizyczna : PosiadaczRachunku
{
    private string imie;
    private string nazwisko;
    private string drugieImie;
    private string PESEL;
    private string numerPaszportu;

    public OsobaFizyczna(string imie_,string nazwisko_,string drugieImie_,string pesel_, string numerPaszportu_){
        if (numerPaszportu_==null && pesel_==null){
            throw new Exception("PESEL albo numer paszportu muszą być nie null");
        }
        if (!string.IsNullOrEmpty(pesel_) && pesel_.Length != 11)
        {
            throw new Exception("PESEL musi mieć 11 cyfr");
        }
        imie=imie_;
        nazwisko=nazwisko_;
        drugieImie=drugieImie_;
        PESEL=pesel_;
        numerPaszportu=numerPaszportu_;
        Console.WriteLine("Utworzono Osobe Fizyczna: "+Imie+" "+ Nazwisko+" ");

    }

    public string Imie
    {
        get {return imie;}
        set {imie=value;}
    }

    public string Nazwisko
    {
        get {return nazwisko;}
        set {nazwisko=value;}
    }

    public string DrugieImie 
    {
        get {return drugieImie;}
        set {drugieImie=value;}
    }

    public string Pesel 
    {
        get {return PESEL;}
        set 
        {
            if (!string.IsNullOrEmpty(value) && value.Length != 11)
            {
                throw new Exception("PESEL musi mieć 11 cyfr");
            }
            PESEL=value;
        }
    }

    public string NumerPaszportu 
    {
        get {return numerPaszportu;}
        set {numerPaszportu=value;}
    }


    public override string ToString()
    {
        return "Osoba fizyczna: "+ Imie + " " +Nazwisko;
    }
}

