﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using Com.OfficerFlake.Libraries.RichText;
using Com.OfficerFlake.Libraries.YSFHQ;

namespace Com.OfficerFlake.Libraries
{
	public static partial class Database
    {
	    public class User
	    {
			public YSFHQConnection YSFHQ = new YSFHQConnection();
		    public RichTextString Username;

		    public User(RichTextString _Username)
		    {
				Can = new PermissionsTesting_Can(this);
				Cannot = new PermissionsTesting_Cannot(this);

			    Username = _Username;
		    }

			#region History
			//Actions on User level.
			public class ExpiringAction
		    {
			    public User ActionedBy;
			    public DateTime Starts;
			    public DateTime Ends;
			    public RichTextString Reason;

			    public ExpiringAction(User actionedBy, DateTime starts, DateTime ends, RichTextString reason)
			    {
				    ActionedBy = actionedBy;
				    Starts = starts;
				    Ends = ends;
				    Reason = reason;
			    }
		    }
		    public class PermanentAction
		    {
			    public User ActionedBy;
			    public DateTime Timestamp;
			    public RichTextString Reason;

			    public PermanentAction(User actionedBy, DateTime timestamp, RichTextString reason)
			    {
				    ActionedBy = actionedBy;
				    Timestamp = timestamp;
				    Reason = reason;
			    }
		    }

			#region Mute
			public ExpiringAction MuteHistory = new ExpiringAction(Users.Console, DateTime.MinValue, DateTime.MinValue, "Never Muted.".AsRichTextString());
		    public bool isMuted => (DateTime.Now < MuteHistory.Ends);
		    void Mute(User mutedBy, DateTime starts, DateTime ends, RichTextString reason)
		    {
			    throw new NotImplementedException();
		    }
			#endregion
			#region Freeze
			public ExpiringAction FreezeHistory = new ExpiringAction(Users.Console, DateTime.MinValue, DateTime.MinValue, "Never Frozen.".AsRichTextString());
		    public bool isFrozen => (DateTime.Now < FreezeHistory.Ends);
			void Freeze(User frozenby, DateTime starts, DateTime ends, RichTextString reason)
		    {
			    throw new NotImplementedException();
		    }
			#endregion
			#region Kick
			public PermanentAction KickHistory = new PermanentAction(Users.Console, DateTime.MinValue,  "Never Kicked.".AsRichTextString());
			void Kick(User kickedby, DateTime timestamp, RichTextString reason)
		    {
				throw new NotImplementedException();
			}
			#endregion
			#region Ban
			public ExpiringAction BanHistory = new ExpiringAction(Users.Console, DateTime.MinValue, DateTime.MinValue, "Never Banned.".AsRichTextString());
		    public bool isBanned => (DateTime.Now < FreezeHistory.Ends);
			void Ban(User bannedby, DateTime starts, DateTime ends, RichTextString reason)
		    {
				throw new NotImplementedException();
			}
			#endregion

		    #region Groups
		    public class GroupReference
		    {
			    public Group Group;
			    public User ActionedBy;
			    public RichTextString Reason;

			    //Actions on Group level.
			    #region Add
			    void AddToGroup(User addedBy, Group groupToAddTo, DateTime timestamp, RichTextString reason)
			    {
				    throw new NotImplementedException();
			    }
			    #endregion
			    #region Remove
			    void RemoveFromGroup(User removedby, Group groupToRemoveFrom, DateTime timestamp, RichTextString reason)
			    {
				    throw new NotImplementedException();
			    }
			    #endregion

			    //Rank
			    #region Rank
			    public class _RankReference
			    {
				    public Rank Rank;
				    public User ActionedBy;
				    public RichTextString Reason;

				    //Actions on Rank level.
				    #region Promote

				    void Promote(User promotedBy, Rank rankToPromoteTo, DateTime timestamp, RichTextString reason)
				    {
					    throw new NotImplementedException();
				    }

				    #endregion
				    #region Demote

				    void Demote(User demotedBy, Rank rankToDemoteTo, DateTime timestamp, RichTextString reason)
				    {
					    throw new NotImplementedException();
				    }

				    #endregion
			    }
			    #endregion
			    public _RankReference RankReference = null;
		    }

		    public Rank GetRankInGroupOrNull(Group target)
		    {
			    try
			    {
				    GroupReference groupReference = Groups.Where(x => x.Group == target).First();
				    return groupReference.RankReference.Rank;
			    }
			    catch
			    {
				    return null;
			    }
		    }
		    #endregion
		    public List<GroupReference> Groups = new List<GroupReference>();
			#endregion
			#region Perissmions
		    private class PermissionsTesting
		    {
			    public PermissionsTesting(User parent)
			    {
				    Parent = parent;
			    }

			    public User Parent;

			    private bool BaseTest(PermissionTypes permissionType, User target, Group scope = null)
			    {
				    #region Initialisation
					User caller = Parent;

				    Rank callerRank;
				    Rank targetRank;

				    Permission minimumPermission;
					Permission permission;

					if (scope == null) scope = Database.Groups.Server;
					#endregion

					#region Minimum Ranks Permission has "Maximum Rank" as -1, and "Must Outrank" as false, so will always have permission. RETURN TRUE.
				    minimumPermission = scope.GetLowestRank().GetPermission(permissionType);
					if (minimumPermission.MaximumRank <= -1 && !minimumPermission.MustOutrank) return true; //permission always enabled.
					#endregion

					#region Caller is not a member of the group. RETURN FALSE.
				    callerRank = caller.GetRankInGroupOrNull(scope);
				    if (callerRank == null) return false; //not a member of the group to begin with...
					#endregion

					#region Test Permissions Failed. RETURN FALSE
					permission = callerRank.GetPermission(permissionType);

					#region Permissions minimum rank must be above -1 to be enabled. RETURN FALSE.
					if (permission.MinimumRank <= -1) return false; //permission disabled.
					#endregion

					#region Caller doesn't outrank target. RETURN FALSE.
					#region Set TargetRank to Minimum if not in the group.
					targetRank = target.GetRankInGroupOrNull(scope);
				    if (targetRank == null) targetRank = scope.GetLowestRank(); //not a member of the group to begin with...
				    #endregion

				    int callerRankIndex = 0;
				    int targetRankIndex = 0;

				    callerRankIndex = callerRank.Index;
				    targetRankIndex = targetRank.Index;

				    bool Outranks = (callerRankIndex > targetRankIndex);
				    if (!Outranks) return false;
				    #endregion
				    #region Targets Rank is outside permissions scope. RETURN FALSE.
				    if (targetRankIndex < permission.MinimumRank) return false; //Target rank is below minimum allowed.
				    if (targetRankIndex > permission.MaximumRank && permission.MaximumRank >= 0) return false; //Target rank is above maximum allowed.
					#endregion
					#endregion

					#region Tested permissions and all is okay. RETURN TRUE.
					return true;
				    #endregion
				}

				public bool Mute(User target)
			    {
				    return BaseTest(PermissionTypes.Mute, target);
			    }
			    public bool Freeze(User target)
			    {
					return BaseTest(PermissionTypes.Freeze, target);
				}
			    public bool Kick(User target)
			    {
					return BaseTest(PermissionTypes.Kick, target);
				}
			    public bool Ban(User target)
			    {
					return BaseTest(PermissionTypes.Ban, target);
				}

			    public bool AddToGroup(User target, Group group)
			    {
				    return BaseTest(PermissionTypes.AddToGroup, target, group);
				}
			    public bool RemoveFromGroup(User target, Group group)
			    {
					return BaseTest(PermissionTypes.RemoveFromGroup, target, group);
				}

			    public bool Promote(User target, Group group)
			    {
					return BaseTest(PermissionTypes.Promote, target, group);
				}
			    public bool Demote(User target, Group group)
			    {
					return BaseTest(PermissionTypes.Demote, target, group);
				}
			}

		    public class PermissionsTesting_Can
		    {
			    private PermissionsTesting Testing;

			    public PermissionsTesting_Can(User parent)
			    {
				    Testing = new PermissionsTesting(parent);
			    }

			    public bool Mute(User target)
			    {
				    return Testing.Mute(target);
			    }
			    public bool Freeze(User target)
			    {
					return Testing.Freeze(target);
				}
			    public bool Kick(User target)
			    {
					return Testing.Kick(target);
				}
			    public bool Ban(User target)
			    {
					return Testing.Ban(target);
				}

			    public bool AddToGroup(User target, Group group)
			    {
					return Testing.AddToGroup(target, group);
				}
			    public bool RemoveFromGroup(User target, Group group)
			    {
					return Testing.RemoveFromGroup(target, group);
				}

			    public bool Promote(User target, Group group)
			    {
					return Testing.Promote(target, group);
				}
			    public bool Demote(User target, Group group)
			    {
					return Testing.Demote(target, group);
				}
			}
		    public PermissionsTesting_Can Can;

		    public class PermissionsTesting_Cannot
		    {
			    private PermissionsTesting Testing;

			    public PermissionsTesting_Cannot(User parent)
			    {
				    Testing = new PermissionsTesting(parent);
			    }

			    public bool Mute(User target)
			    {
				    return !Testing.Mute(target);
			    }
			    public bool Freeze(User target)
			    {
				    return !Testing.Freeze(target);
			    }
			    public bool Kick(User target)
			    {
				    return !Testing.Kick(target);
			    }
			    public bool Ban(User target)
			    {
				    return !Testing.Ban(target);
			    }

			    public bool AddToGroup(User target, Group group)
			    {
				    return !Testing.AddToGroup(target, group);
			    }
			    public bool RemoveFromGroup(User target, Group group)
			    {
				    return !Testing.RemoveFromGroup(target, group);
			    }

			    public bool Promote(User target, Group group)
			    {
				    return !Testing.Promote(target, group);
			    }
			    public bool Demote(User target, Group group)
			    {
				    return !Testing.Demote(target, group);
			    }
			}
		    public PermissionsTesting_Cannot Cannot;
			#endregion
		}

	    public static class Users
	    {
		    public static User Unknown = new User("&o&4<UNKNOWN>".AsRichTextString());
		    public static User Console = new User("&o&3<OpenYS Console>".AsRichTextString())
		    {
			    Groups = new List<User.GroupReference>()
			    {
				    new User.GroupReference()
				    {
					    Group = Groups.Server,
						RankReference = new User.GroupReference._RankReference()
						{
							Rank = Ranks.ServerRankAdmin
						}
				    }
			    }
		    };
		    public static User TestUser = new User("TestUser".AsRichTextString())
		    {
			    Groups = new List<User.GroupReference>()
			    {
				    new User.GroupReference()
				    {
					    Group = Groups.Server,
					    RankReference = new User.GroupReference._RankReference()
					    {
						    Rank = Ranks.ServerRankGuest
						}
				    }
			    }
		    };
	    }
    }
}