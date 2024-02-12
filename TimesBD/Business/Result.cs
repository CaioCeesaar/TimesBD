namespace TimesBD.Business
{
    public class Result
    {
        public Result(bool sucess, string message)
        {
            Sucess = sucess;
            Message = message;
        }

        public bool Sucess { get; set; }

        public string Message { get; set; }
    }
}
