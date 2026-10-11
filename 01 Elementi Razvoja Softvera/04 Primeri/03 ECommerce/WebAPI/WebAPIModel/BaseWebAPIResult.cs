namespace WebAPI.WebAPIModel
{
    public abstract class BaseWebAPIResult
    {
        public string Message { get; set; }

        public BaseWebAPIResult()
        {
            InitProps();
            this.Message = string.Empty;
        }


        public BaseWebAPIResult(string message)
        {
            InitProps();
            this.Message = message;
        }

        protected abstract void InitProps();
    }
}
