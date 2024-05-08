using System.Numerics;
using System.Text;
using System.Xml.Schema;

public class RachunekBankowy
{
    private string numer;
    private decimal stanRachunku;
    private bool czyDozwolonyDebet;
    private List<PosiadaczRachunku>_PosiadaczeRachunku = new List<PosiadaczRachunku>();
    private static List<Transakcja> _Transakcje = new();
    public string Numer
    {
        get {return numer;}
        set {numer=value;}
    }

    public decimal StanRachunku
    {
        get {return stanRachunku;}
        set { stanRachunku=value;}
    }
    
    public bool CzyDozwolonyDebet 
    {
        get {return czyDozwolonyDebet;}
        set {czyDozwolonyDebet=value;}
    }

    public List<PosiadaczRachunku> PosiadaczRachunku_
    {
        get {return _PosiadaczeRachunku;}
        set {_PosiadaczeRachunku=value;}
    }

    public RachunekBankowy(string numer_,decimal stanRachunku_, bool czyDozwolonyDebet_,List<PosiadaczRachunku>_PosiadaczeRachunku_){
        if (_PosiadaczeRachunku_.Count==0)
        {
            throw new Exception("Nie moze istniec konto bez posiadacza");
        }
        numer=numer_;
        stanRachunku=stanRachunku_;
        czyDozwolonyDebet=czyDozwolonyDebet_;
        _PosiadaczeRachunku=_PosiadaczeRachunku_;
    }
    public static void DokonajTransakcji( RachunekBankowy rachunekZrodlowy_, RachunekBankowy rachunekDocelowy_,decimal kwota_, string opis_)
    {
        if (kwota_ < 0 || (rachunekZrodlowy_ == null && rachunekDocelowy_ == null))
        {
            throw new ArgumentException("Kwota nie może być ujemna, a rachunki nie mogą być null");
        }
        if (rachunekZrodlowy_ != null && !rachunekZrodlowy_.CzyDozwolonyDebet && kwota_ > rachunekZrodlowy_.StanRachunku)
        {
            throw new ArgumentException("Rachunek źródłowy nie pozwala na debet, a kwota transakcji przekracza stan rachunku");
        }
        if (rachunekZrodlowy_==null)
        {
            rachunekDocelowy_.stanRachunku+=kwota_;
            Transakcja wplataGotowkowa = new Transakcja(rachunekZrodlowy_, rachunekDocelowy_, kwota_);
            _Transakcje.Add(wplataGotowkowa);
        }
        else if (rachunekDocelowy_ == null)
        {
            rachunekZrodlowy_.stanRachunku -= kwota_;
            Transakcja wyplataGotowkowa = new Transakcja(rachunekZrodlowy_, rachunekDocelowy_, kwota_);
            _Transakcje.Add(wyplataGotowkowa);
        }
        else
        {
            rachunekZrodlowy_.stanRachunku -= kwota_;
            rachunekDocelowy_.stanRachunku += kwota_;
            Transakcja przelew = new Transakcja(rachunekZrodlowy_, rachunekDocelowy_, kwota_);
            _Transakcje.Add(przelew);
        }
    }
    public static RachunekBankowy operator +(RachunekBankowy rachunek, PosiadaczRachunku posiadacz)
    {
        if (rachunek._PosiadaczeRachunku.Contains(posiadacz))
        {
            throw new ArgumentException("Posiadacz już istnieje na liście posiadaczy rachunku");
        }
        rachunek._PosiadaczeRachunku.Add(posiadacz);
        
        return rachunek;
    }
    public static RachunekBankowy operator -(RachunekBankowy rachunek, PosiadaczRachunku posiadacz)
    {
        if (rachunek._PosiadaczeRachunku.Count-1<1){
            throw new Exception("Nie mozna usunac ostatniego posiadacza rachunku bankowego");
        }    
        if (!rachunek._PosiadaczeRachunku.Contains(posiadacz))
        {
            throw new Exception("Ta osoba nie jest posiadaczem tego rachunku");
        }
        rachunek._PosiadaczeRachunku.Remove(posiadacz);
        return rachunek;
    }
public override string ToString()
{
    StringBuilder builder = new StringBuilder();

    builder.Append("Numer rachunku: ").Append(numer)
            .Append("\n")
           .Append(". Aktualny stan rachunku: ").Append(stanRachunku)
           .Append("\n")
           .Append(". Posiadacze konta:")
            .Append("\n");

    foreach (PosiadaczRachunku posiadacz in _PosiadaczeRachunku)
    {
        builder.Append(' ').Append(posiadacz.ToString()).Append(",");
    }
    builder.Append("\n");

    builder.Remove(builder.Length - 1, 1); // Usunięcie ostatniego przecinka

        builder.Append("\n");

    foreach (Transakcja transakcja in _Transakcje)
    {
        builder.Append(' ').Append(transakcja.ToString()).Append(",");
    }

    builder.Remove(builder.Length - 1, 1); 
    
    


    return builder.ToString();
}

}