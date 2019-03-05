namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Services
{
    using System;

    public class RoomService : IRoomService
    {
        public int GetRoomFloorByNumber(decimal roomNumber)
        {
            return Convert.ToInt32(Math.Floor(roomNumber));
        }
    }
}
