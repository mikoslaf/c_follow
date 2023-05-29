using System;
using System.Collections.Generic;
using System.Dynamic;
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
        private Ped[] peds = new Ped[0];
        public ClientMain()
        {
            TriggerEvent("chat:addSuggestion", "/follow", "Create npc to follow/open menu");


            API.RegisterCommand("follow", new Action<int, List<object>, string>(follow), false);

            //TriggerEvent("chat:addSuggestion", "/follow-anim", "Tast play anim for NPC or Reset following", new[] 
            //{
            // new { name = "dist", help = "dist"},
            // new { name = "anim", help = "anim"},
            //});

            //API.RegisterCommand("follow-anim", new Action<int, List<object>, string>(follow_anim), false);

            API.RegisterNuiCallbackType("c_spawn");
            EventHandlers["__cfx_nui:c_spawn"] += new Action<ExpandoObject>(spawn);

            API.RegisterNuiCallbackType("c_anim");
            EventHandlers["__cfx_nui:c_anim"] += new Action<ExpandoObject>(anim);

            API.RegisterNuiCallbackType("c_delete");
            EventHandlers["__cfx_nui:c_delete"] += new Action(delete);

            API.RegisterNuiCallbackType("c_kill");
            EventHandlers["__cfx_nui:c_kill"] += new Action(kill);

            API.RegisterNuiCallbackType("c_cancel");
            EventHandlers["__cfx_nui:c_kill"] += new Action(cancel);

            API.RegisterNuiCallbackType("c_follow");
            EventHandlers["__cfx_nui:c_follow"] += new Action(follow_again);
        }

        private async void spawn(dynamic data) 
        {
            API.SetNuiFocus(false, false);
            String Model = data.model;
            byte cont = Convert.ToByte(data.cont);
            bool armed = data.armed;
            string weapon = data.weapon;
            bool combat = data.combat;

            Ped player = Game.Player.Character;
            peds = new Ped[cont];

            if (Model == "random")
            {
                String[] Names = { "a_f_m_trampbeac_01", "a_f_y_eastsa_03", "a_f_y_hipster_04", "a_m_m_genfat_01", "a_m_m_salton_02", "a_m_y_beachvesp_01", "a_m_y_clubcust_01", "a_m_y_polynesian_01" };
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
                    //npc.Task.LookAt(player);
                    npc.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);

                    API.SetPedAsGroupMember(npc.Handle, API.GetPedGroupIndex(npc.Handle));
                    API.SetPedCombatAbility(npc.Handle, 2);
                    peds[i] = npc;
                }
            }
            else
            {
                uint Hash = (uint)GetHashKey(Model);
                API.RequestModel(Hash);
                while (!API.HasModelLoaded(Hash))
                {
                    await BaseScript.Delay(100);
                }
                for (int i = 0; i < peds.Length; i++)
                {
                    Ped npc = await World.CreatePed((Model) Model, player.Position + (player.ForwardVector * 2));
                    //npc.Task.LookAt(player);
                    npc.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);

                    API.SetPedAsGroupMember(npc.Handle, API.GetPedGroupIndex(npc.Handle));
                    API.SetPedCombatAbility(npc.Handle, 2);
                    peds[i] = npc;
                }
            }
        }

        private async void anim(dynamic data) 
        {
            API.SetNuiFocus(false, false);
            String dist = data.dist;
            String anim = data.anim;

            while (!API.HasAnimDictLoaded(dist))
            {
                API.RequestAnimDict(dist);
                await BaseScript.Delay(100);
            }
            AnimationFlags flags = AnimationFlags.Loop | AnimationFlags.CancelableWithMovement;
            foreach (Ped i in peds)
            {
                i.Task.ClearAllImmediately();
                i.Task.PlayAnimation(dist, anim, -1, -1, flags);
            }

        }

        private void follow_again()
        {
            API.SetNuiFocus(false, false);
            Ped player = Game.Player.Character;
            foreach (Ped i in peds)
            {
                i.Task.ClearAllImmediately();
                i.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);
            }
        }

        private void delete() 
        {
            API.SetNuiFocus(false, false);
            foreach (Ped i in peds)
            {
                i.Delete();
            }
            peds = new Ped[0];
        }
        private void kill()
        {
            API.SetNuiFocus(false, false);
            foreach (Ped i in peds)
            {
                i.Kill();
            }
            peds = new Ped[0];
        }

        private void cancel()
        {
            API.SetNuiFocus(false, false);
        }

        private void follow(int source, List<object> args, string raw)
        {
            API.SetNuiFocus(true, true);
            if (peds.Length > 0) 
            {
                SendNuiMessage("{\"action\":\"menu\"}");
            } 
            else
            {
                SendNuiMessage("{\"action\":\"start\"}");
            }
            
            //if ((bool)args.Any())
            //{
            //    if (peds.Length != 0)
            //    {
            //        foreach (Ped i in peds)
            //        {
            //            i.Delete();
            //        }
            //    }
            //    byte cont = 4;
            //    if (args.ElementAtOrDefault(1) != null)
            //    {
            //        if (byte.TryParse(args[1].ToString(), out _))
            //        {
            //            cont = Convert.ToByte(args[1]);
            //            if (cont < 0 && cont > 40) //zmieniæ !!!!
            //            {
            //                cont = 4;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (Ped i in peds)
            //    {
            //        i.Delete();
            //    }
            //    peds = new Ped[0];
            //}

        }

        //private async void follow_anim(int source, List<object> args, string raw)
        //{
        //    String ani1 = "", ani2 = "";
        //    if (args.Count >= 2)
        //    {
        //        ani1 = args[0].ToString();
        //        ani2 = args[1].ToString();
        //    }
        //    if (ani1 == "" && ani2 == "")
        //    {
        //        Ped player = Game.Player.Character;
        //        foreach (Ped i in peds)
        //        {
        //            i.Task.ClearAllImmediately();
        //            i.Task.FollowToOffsetFromEntity(player, (player.ForwardVector * 2), -1, 10);
        //        }
        //    }
        //    while (!API.HasAnimDictLoaded(ani1))
        //    {
        //        API.RequestAnimDict(ani1);
        //        await BaseScript.Delay(100);
        //    }
        //    AnimationFlags flags = AnimationFlags.Loop | AnimationFlags.CancelableWithMovement;
        //    foreach (Ped i in peds)
        //    {
        //        i.Task.ClearAllImmediately();
        //        i.Task.PlayAnimation(ani1, ani2, -1, -1, flags);
        //    }

        //}
    }
}