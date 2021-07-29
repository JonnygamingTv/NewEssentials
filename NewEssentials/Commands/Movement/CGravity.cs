﻿using Cysharp.Threading.Tasks;
using OpenMod.API.Permissions;
using OpenMod.Core.Commands;
using OpenMod.Core.Permissions;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;

namespace NewEssentials.Commands.Movement
{
    [Command("gravity")]
    [CommandDescription("Change your or other player gravity. 0 = Freeze, 1 = Normal")]
    [CommandSyntax("[user] <gravity>")]
    [RegisterCommandPermission("other", Description = "Allows to change gravity of other player")]
    public class CGravity : UnturnedCommand
    {
        public CGravity(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async UniTask OnExecuteAsync()
        {
            var targetActor = Context.Parameters.Count == 2
                ? await Context.Parameters.GetAsync<UnturnedUser>(0)
                : (UnturnedUser)Context.Actor;

            if (targetActor != Context.Actor && await CheckPermissionAsync("other") != PermissionGrantResult.Grant)
            {
                throw new NotEnoughPermissionException(Context, "other");
            }

            if (targetActor == null)
            {
                return;
            }

            var gravity = await Context.Parameters.GetAsync<float>(Context.Parameters.Count - 1);

            await UniTask.SwitchToMainThread();
            targetActor.Player.Player.movement.sendPluginGravityMultiplier(gravity);
        }
    }
}
