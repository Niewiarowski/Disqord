﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class RestMembersRequestEnumerator : RestRequestEnumerator<RestMember>
    {
        private readonly Snowflake _guildId;
        private readonly Snowflake? _startFromId;

        public RestMembersRequestEnumerator(RestDiscordClient client,
            Snowflake guildId, int limit, Snowflake? startFromId) : base(client, 1000, limit)
        {
            _guildId = guildId;
            _startFromId = startFromId;
        }

        protected override Task<IReadOnlyList<RestMember>> NextPageAsync(
            IReadOnlyList<RestMember> previous, RestRequestOptions options = null)
        {
            var amount = Remaining > 100
                ? 100
                : Remaining;
            var startFromId = _startFromId;
            if (previous != null && previous.Count > 0)
                startFromId = previous.Max(x => x.Id);

            return Client.InternalGetMembersAsync(_guildId, amount, startFromId, options);
        }
    }
}