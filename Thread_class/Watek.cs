class Watek{
    private int threadNumber;
    private AutoResetEvent endSignal;

    public Watek(int threadNumber, AutoResetEvent endSignal)
    {
        this.threadNumber = threadNumber;
        this.endSignal = endSignal;
    }

    public void DoWork()
    {
        Console.WriteLine("Wątek {0} rozpoczął działanie.", threadNumber);

        Thread.Sleep(1000);

        endSignal.Set();
    }
}
