namespace ends.tower.mess
{
    [System.Serializable]
    public struct Test
    {
        public const short OPCODE = 1000;
        public string message;
    }

    [System.Serializable]
    public struct Test_Reply
    {
        public string response;
    }
}