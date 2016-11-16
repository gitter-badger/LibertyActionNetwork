using Li.Lan.Common.Data;
using Li.Lan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Li.Lan.Data
{
    public interface IUserRepository
    {
        IEnumerable<UserProfile> SelectUserProfiles(UserProfileSearchCriteria userProfileSearchCriteria);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConnectionStringProvider connectionStringProvider)
            : base(connectionStringProvider)
        {
            // no-op
        }

        public IEnumerable<UserProfile> SelectUserProfiles(UserProfileSearchCriteria userProfileSearchCriteria)
        {
            IEnumerable<UserProfile> result;

            using (var ctx = this.CreateUserContext())
            {
                var query =
                    from u in ctx.UserProfiles.Include("Roles").Include("PrecinctTags")
                    select u;

                // filter by UserName if provided
                if (!String.IsNullOrWhiteSpace(userProfileSearchCriteria.UserName))
                {
                    var userNameUpper = userProfileSearchCriteria.UserName.ToUpper();
                    query = query.Where(x => x.UserName.ToUpper().Contains(userNameUpper));
                }

                // filter by Role if provided
                if (!String.IsNullOrWhiteSpace(userProfileSearchCriteria.RoleName))
                {
                    query = query.Where(x => x.Roles.Select(r => r.RoleName).Contains(userProfileSearchCriteria.RoleName));
                }

                // limit results if ResultLimit provided
                if (userProfileSearchCriteria.ResultLimit.HasValue)
                {
                    query = query.Take(userProfileSearchCriteria.ResultLimit.Value);
                }

                result = query.OrderBy(x => x.UserName).ToList();
            }

            return result;
        }

        private UserContext CreateUserContext()
        {
            return new UserContext(this.ConnectionStringProvider.GetConnectionString());
        }
    }
}