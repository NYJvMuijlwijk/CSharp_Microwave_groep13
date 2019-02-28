namespace Microwave.Interfaces
{
    public interface IItem
    {
        void Idle(bool open);
        void Open();
        void Close();
        void Cook();
        void Done();
        void Add();
        void Remove();
    }
}