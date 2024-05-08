public class Transakcja
{
    private RachunekBankowy rachunekZrodlowy;
    private RachunekBankowy rachunekDocelowy;
    private decimal kwota;

    public RachunekBankowy RachunekZrodlowy
    {
        get {return rachunekZrodlowy;}
        set {rachunekZrodlowy=value;}
    }
    public RachunekBankowy RachunekDocelowy 
    {
        get {return rachunekDocelowy;}
        set {rachunekDocelowy=value;}
    }
    public decimal Kwota 
    {
        get {return kwota;}
        set {kwota=value;}
    }
    public Transakcja(RachunekBankowy rachunekZrodlowy_, RachunekBankowy rachunekDocelowy_, decimal kwota_){
        if (rachunekDocelowy_==null && rachunekZrodlowy_==null){
            throw new Exception("Rachunek Źródłowy ani Docelowy nie mogą być nullem");
        }
        rachunekDocelowy=rachunekDocelowy_;
        rachunekZrodlowy=rachunekZrodlowy_;
        kwota=kwota_;
    }
    public override string ToString()
{   
    if (rachunekZrodlowy== null){
                return "Wpłata na rachunek:"+ rachunekDocelowy. Numer +" Kwota: " + kwota;
    }
    if (rachunekDocelowy==null){
        return "Wyplata z rachunku: Numer+ " + rachunekDocelowy. Numer +" Kwota: " + kwota;
    }
    else
        return "Transakcja: Z rachunku: " + rachunekZrodlowy.Numer+ " na rachunek "+ rachunekDocelowy.Numer +" Kwota: " + kwota;
    }

}