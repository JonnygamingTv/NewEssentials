﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewEssentials.API;
using OpenMod.API.Ioc;
using SDG.Unturned;

namespace NewEssentials.Managers
{
    [ServiceImplementation]
    public class PrivateMessageManager : IPrivateMessageManager, IAsyncDisposable
    {
        private readonly Dictionary<ulong, ulong> m_LastMessage;

        public PrivateMessageManager()
        {
            Provider.onEnemyDisconnected += RemovePlayer;
        }
        
        public void RecordLastMessager(ulong recipient, ulong sender) => m_LastMessage.Add(recipient, sender);

        public ulong? GetLastMessager(ulong recipient)
        {
            if (!m_LastMessage.ContainsKey(recipient))
                return null;

            return m_LastMessage[recipient];
        }

        public async ValueTask DisposeAsync()
        {
            Provider.onEnemyDisconnected -= RemovePlayer;
            await Task.Yield();
        }

        private void RemovePlayer(SteamPlayer gonePlayer) =>
            m_LastMessage.Remove(gonePlayer.playerID.steamID.m_SteamID);
    }
}