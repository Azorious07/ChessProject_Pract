namespace Chess.Model.Command
{
    public interface ICommandVisitable
    {
        void Accept(ICommandVisitor visitor);

        T Accept<T>(ICommandVisitor<T> visitor);
    }
}