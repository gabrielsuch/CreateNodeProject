namespace CreateNodeProject
{
    class CreateFileObject
    {
        public string path;
        public string filename;
        public string content;

        public CreateFileObject(string path, string filename, string content)
        {
            this.path = path;
            this.filename = filename;
            this.content = content;
        }
    }
}
