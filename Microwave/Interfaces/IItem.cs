namespace Microwave
{
    public interface IItem
    {
        void Idle(bool isOpen);
        void Open();
        void Close();
        void Cook();
        void Done();
        void Add();
        void Remove();
    }
}