﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        public Task<List<string>> GetConnectionsForUser(string userId)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(userId);
            }

            return Task.FromResult(connectionIds);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<bool> UserConnected(string userId, string connectionId)
        {
            bool isOnline = false;
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(userId))
                {
                    OnlineUsers[userId].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(userId, new List<string>()
                    {
                        connectionId
                    });
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string userId, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(userId))
                    return Task.FromResult(isOffline);

                OnlineUsers[userId].Remove(connectionId);

                if (OnlineUsers[userId].Count == 0)
                {
                    OnlineUsers.Remove(userId);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }
    }
}