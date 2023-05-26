using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace c_follow.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Ped[] peds = new Ped[0];
            TriggerEvent("chat:addSuggestion", "/follow", "Create npc to follow", new[] { 
             new { name = "Model", help = "Name Model for NPC or [Random]"},
             new { name = "Number", help = "Number of NPC [1-10]"},
            });
            API.RegisterCommand("follow", new Action<int, List<object>, string>(async (source, args, rawCommand) =>
            {
                if ((bool)args.Any())
                {
                    if (peds.Length != 0) 
                    {
                        foreach (Ped i in peds)
                        {
                            i.Delete();
                        }
                    }
                    byte cont = 4;
                    if (args.ElementAtOrDefault(1) != null) 
                    {
                        if (byte.TryParse(args[1].ToString(), out _)) {
                            cont = Convert.ToByte(args[1]);
                            if (cont < 0 && cont > 40) //zmieniæ !!!!
                            {
                                cont = 4;
                            }
                        }
                    }

                    Ped player = Game.Player.Character;
                    peds = new Ped[cont];

                    if (args[0].ToString() == "Random") 
                    {
                        String[] Names = { "a_f_m_trampbeac_01", "a_f_y_eastsa_03", "a_f_y_hipster_04", "a_m_m_genfat_01", "a_m_m_salton_02", "a_m_y_beachvesp_01", "a_m_y_clubcust_01", "a_m_y_polynesian_01"};
                        Random rnd = new Random();

                        for (int i = 0; i < peds.Length; i++)
                        {
                            Byte rand = (byte)rnd.Next(0, Names.Length);
                            uint Hash = (uint)GetHashKey(Names[rand]);
                            API.RequestModel(Hash);
                            while (!API.HasModelLoaded(Hash))
                            {
                                await BaseScript.Delay(100);
                            }

                            Ped npc = await World.CreatePed((Model)Names[rand], player.Position + (player.ForwardVector * 2));
                            npc.Task.LookAt(player);
                            npc.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);

                            API.SetPedAsGroupMember(npc.Handle, API.GetPedGroupIndex(npc.Handle));
                            API.SetPedCombatAbility(npc.Handle, 2);
                            peds[i] = npc;
                        }
                    } else
                    {
                        uint Hash = (uint)GetHashKey(args[0].ToString());
                        API.RequestModel(Hash);
                        while (!API.HasModelLoaded(Hash))
                        {
                            await BaseScript.Delay(100);
                        }
                        for (int i = 0; i < peds.Length; i++)
                        {
                            Ped npc = await World.CreatePed((Model)args[0].ToString(), player.Position + (player.ForwardVector * 2));
                            //npc.Task.LookAt(player);
                            npc.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);

                            API.SetPedAsGroupMember(npc.Handle, API.GetPedGroupIndex(npc.Handle));
                            API.SetPedCombatAbility(npc.Handle, 2);
                            peds[i] = npc;
                        }
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
            API.RegisterCommand("follow-anim", new Action<int, List<object>, string>(async (source, args, rawCommand) =>
            {
                String ani1 = "", ani2 = "";
                if (args.Count >= 2) 
                {
                    ani1 = args[0].ToString();
                    ani2 = args[1].ToString();
                }
                if (ani1 == "" && ani2 == "") 
                {
                    Ped player = Game.Player.Character;
                    foreach (Ped i in peds)
                    {
                        i.Task.ClearAllImmediately();
                        i.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);
                    }
                }
                while (!API.HasAnimDictLoaded(ani1)) 
                { 
                    API.RequestAnimDict(ani1);
                    await BaseScript.Delay(100);
                }
                AnimationFlags flags = AnimationFlags.Loop | AnimationFlags.CancelableWithMovement;
                foreach (Ped i in peds)
                {
                    i.Task.ClearAllImmediately();
                    i.Task.PlayAnimation(ani1, ani2, -1, -1, flags);
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