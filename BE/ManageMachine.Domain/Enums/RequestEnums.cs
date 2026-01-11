namespace ManageMachine.Domain.Enums
{
    public enum MachineStatus
    {
        Available = 0,
        Borrowed = 1,
        Repairing = 2
    }

    public enum RequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public enum RequestType
    {
        Borrow = 0,
        Repair = 1,
        Return = 2
    }
}
