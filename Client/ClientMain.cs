using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;

namespace c_follow.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Ped[] peds = new Ped[0]; 
            API.RegisterCommand("follow", new Action<int, List<object>, string>(async (source, args, rawCommand) =>
            {
                if ((bool)args.Any())
                {
                    byte cont = 4;
                    if (args.ElementAtOrDefault(1) != null) 
                    {
                        Debug.WriteLine(args[1].ToString());
                        if (int.TryParse(args[1].ToString(), out _)) {
                            if (Enumerable.Range(1, 10).Contains((int)args[1]))
                            {
                                cont = (byte)args[1];
                            }
                        }
                    }
                    uint Hash = (uint)GetHashKey(args[0].ToString());
                    //Model Hash = PedHash;
                    //Debug.WriteLine(Hash.ToString());
                    //Debug.WriteLine();
                    Ped player = Game.Player.Character;
                    API.RequestModel(Hash);
                    peds = new Ped[cont];
                    while (!API.HasModelLoaded(Hash))
                    {
                        await BaseScript.Delay(100);
                    }
                    for (int i = 0; i < peds.Length; i++)
                    {
                        Ped npc = await World.CreatePed((Model)args[0].ToString(), player.Position + (player.ForwardVector * 2));
                        npc.Task.LookAt(player);
                        npc.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);

                        API.SetPedAsGroupMember(npc.Handle, API.GetPedGroupIndex(npc.Handle));
                        API.SetPedCombatAbility(npc.Handle, 2);
                        peds[i] = npc;
                    }
                }
                else { 
                    foreach (Ped i in peds)
                    {
                        i.Delete();
                    }
                    peds = new Ped[0];
                }

            }), false);
        }



        //[Tick]
        //public Task OnTick()
        //{
        //    DrawRect(0.5f, 0.5f, 0.5f, 0.5f, 255, 255, 255, 150);

        //    return Task.FromResult(0);
        //}
    }
}