//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rock.Models.Cms;
using Rock.Repository.Cms;

namespace Rock.Services.Cms
{
    public partial class AuthService : Rock.Services.Service
    {
        private IAuthRepository _repository;

        public AuthService()
			: this( new EntityAuthRepository() )
        { }

        public AuthService( IAuthRepository AuthRepository )
        {
            _repository = AuthRepository;
        }

        public IQueryable<Rock.Models.Cms.Auth> Queryable()
        {
            return _repository.AsQueryable();
        }

        public Rock.Models.Cms.Auth GetAuth( int id )
        {
            return _repository.FirstOrDefault( t => t.Id == id );
        }
		
        public IEnumerable<Rock.Models.Cms.Auth> GetAuthsByEntityTypeAndEntityId( string entityType, int? entityId )
        {
            return _repository.Find( t => t.EntityType == entityType && ( t.EntityId == entityId || ( entityId == null && t.EntityId == null ) ) ).OrderBy( t => t.Order );
        }
		
        public IEnumerable<Rock.Models.Cms.Auth> GetAuthsByGuid( Guid guid )
        {
            return _repository.Find( t => t.Guid == guid ).OrderBy( t => t.Order );
        }
		
        public void AddAuth( Rock.Models.Cms.Auth Auth )
        {
            if ( Auth.Guid == Guid.Empty )
                Auth.Guid = Guid.NewGuid();

            _repository.Add( Auth );
        }

        public void AttachAuth( Rock.Models.Cms.Auth Auth )
        {
            _repository.Attach( Auth );
        }

		public void DeleteAuth( Rock.Models.Cms.Auth Auth )
        {
            _repository.Delete( Auth );
        }

        public void Save( Rock.Models.Cms.Auth Auth, int? personId )
        {
            List<Rock.Models.Core.EntityChange> entityChanges = _repository.Save( Auth, personId );

			if ( entityChanges != null )
            {
                Rock.Services.Core.EntityChangeService entityChangeService = new Rock.Services.Core.EntityChangeService();

                foreach ( Rock.Models.Core.EntityChange entityChange in entityChanges )
                {
                    entityChange.EntityId = Auth.Id;
                    entityChangeService.AddEntityChange ( entityChange );
                    entityChangeService.Save( entityChange, personId );
                }
            }
        }

        public void Reorder( List<Rock.Models.Cms.Auth> Auths, int oldIndex, int newIndex, int? personId )
        {
            Rock.Models.Cms.Auth movedAuth = Auths[oldIndex];
            Auths.RemoveAt( oldIndex );
            if ( newIndex >= Auths.Count )
                Auths.Add( movedAuth );
            else
                Auths.Insert( newIndex, movedAuth );

            int order = 0;
            foreach ( Rock.Models.Cms.Auth Auth in Auths )
            {
                if ( Auth.Order != order )
                {
                    Auth.Order = order;
                    Save( Auth, personId );
                }
                order++;
            }
        }
    }
}
