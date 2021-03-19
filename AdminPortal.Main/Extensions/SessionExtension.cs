using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;
using TKW.ApplicationCore.Contexts.AccountContext.DTOs;
using TKW.ApplicationCore.Contexts.FranchiseContext.DTOs;

namespace TKW.AdminPortal.Extensions
{
    public static class SessionExtension
    {
        public static void SetAuthTicket(this ISession session, AuthenticationTicket ticket)
        {
            session.Set("AuthTicket", TicketSerializer.Default.Serialize(ticket));
        }

        public static AuthenticationTicket GetAuthTicket(this ISession session)
        {
            var ticket = session.Get("AuthTicket");
            return (ticket == null || ticket.Length <= 0) ? null : TicketSerializer.Default.Deserialize(ticket);
        }

        public static void DeleteAuthTicket(this ISession session)
        {
            session.Remove("AuthTicket");
        }

        public static UserModel GetUser(this ISession session)
        {
            var user = session.GetString("User");
            if (string.IsNullOrWhiteSpace(user)) return null;
            return JsonSerializer.Deserialize<UserModel>(user);
        }
        public static void SetUser(this ISession session, UserModel user)
        {
            if (user != null)
            {
                session.SetString("User", JsonSerializer.Serialize(user));
            }
        }

        public static void DeleteUser(this ISession session)
        {
            session.Remove("User");
            session.Remove("AuthTicket");
        }

        public static void SetFranchises(this ISession session, List<FranchiseModel> franchises)
        {
            if (franchises != null)
            {
                session.SetString("Franchises", JsonSerializer.Serialize(franchises));
            }
        }

        public static List<FranchiseModel> GetFranchises(this ISession session)
        {
            var franchises = session.GetString("Franchises");
            if (string.IsNullOrEmpty(franchises)) return null;
            return JsonSerializer.Deserialize<List<FranchiseModel>>(franchises);
        }

    }
}
