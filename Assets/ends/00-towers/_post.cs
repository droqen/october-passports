namespace ends.tower.post
{

    using navdi3;

    [System.Serializable]
    public struct RequestStories
    {
        public const short OPCODE = 1000;
        public string message;
    }

    [System.Serializable]
    public struct Test_Reply
    {
        public string response;
    }

    [System.Serializable]
    public struct WorldMove
    {
        public const short OPCODE = 1002;
        public twin dir;
    }

    [System.Serializable]
    public struct BC_ChatMessage
    {
        public const short OPCODE = 2000;

        public string speaker;
        public string message;
    }
}