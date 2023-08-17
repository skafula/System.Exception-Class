internal class Program
{
    class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private double _currentBalance;
        public double CurrentBalance
        {
            get => _currentBalance;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"Invalid amount to add. Amount must be higher than 0. You tried to add {value}.");
                _currentBalance = value;
            }
        }
    }
    class TransferFunds
    {
        public void Transfer(BankAccount accountFrom, BankAccount accountTo, double amount)
        {
            try
            {
                if (amount <0 || amount > 10000)
                    throw new ArgumentOutOfRangeException("amount", amount, "For transfer funds, amount must be between 1 and 10000");
                if (amount > accountFrom.CurrentBalance)
                    throw new InsufficientFundsException($"Insufficient balance at the source account. Balance: {accountFrom.CurrentBalance}.");

                accountFrom.CurrentBalance -= amount;
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentNullException("You have supplied null value to 'accountFrom'", ex);
            }
            try
            {
                accountTo.CurrentBalance += amount;
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentNullException("You have supplied null value to 'accountTo'", ex);
            }
        }
    }
    class InsufficientFundsException : ArgumentException
    {
        public InsufficientFundsException() { }
        public InsufficientFundsException(string message) : base(message) { }
        public InsufficientFundsException(string message, Exception exception) : base(message, exception) { }
    }
    class ExceptionLogger
    {
        public static void LogException(Exception ex)
        {
            string filePath = @"c:\FileTest\exceptionLog.txt";
            StreamWriter streamWriter = File.AppendText(filePath);
            streamWriter.WriteLine("Creation date: " + DateTime.Now);
            streamWriter.WriteLine("Exception type: " + ex.GetType().ToString());
            streamWriter.WriteLine("StackTrace: ");
            streamWriter.WriteLine(ex.StackTrace);
            streamWriter.WriteLine("Message: ");
            streamWriter.WriteLine(ex.Message);
            streamWriter.WriteLine("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
            streamWriter.Close();
        }
    }
    private static void Main(string[] args)
    {
        try
        {
            BankAccount account1 = new BankAccount() { Id = 101, Name = "Bob", CurrentBalance = 6000 };
            BankAccount account2 = new BankAccount() { Id = 102, Name = "Jessica", CurrentBalance = 3000 };
            BankAccount account3 = null;
            TransferFunds transferFunds = new TransferFunds();

            Console.WriteLine($"Id: {account1.Id} Name: {account1.Name} Balance: {account1.CurrentBalance}");
            Console.WriteLine($"Id: {account2.Id} Name: {account2.Name} Balance: {account2.CurrentBalance}");

            transferFunds.Transfer(account1, account3, 4000);


            Console.WriteLine("Funds transferred.");
            Console.WriteLine("After transfer: ");
            Console.WriteLine($"Id: {account1.Id} Name: {account1.Name} Balance: {account1.CurrentBalance}");
            Console.WriteLine($"Id: {account2.Id} Name: {account2.Name} Balance: {account2.CurrentBalance}");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null )
                Console.WriteLine(ex.InnerException.Message);
            ExceptionLogger.LogException(ex);
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "amount")
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.ActualValue);
            Console.WriteLine(ex.ParamName);
            ExceptionLogger.LogException(ex);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine(ex.Message);

            //Stack-trace is useful for developer to see exactly where the exception occured
            Console.WriteLine(ex.StackTrace);
            ExceptionLogger.LogException(ex);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            ExceptionLogger.LogException(ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            ExceptionLogger.LogException(ex);
        }
        //finally
        //{
        //    Console.WriteLine(nameof(BankAccount.CurrentBalance));
        //}
    }
}