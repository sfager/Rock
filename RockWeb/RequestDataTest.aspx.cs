using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Checkin.DTO;
using Rock.CRM;
using Rock.Core;
using Rock.Groups;

public partial class RequestDataTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		RequestData2 rd = new RequestData2();
		Group family = new Group();
		family.Members.Add( new Member { PersonId = 1 } );
		family.Members.Add( new Member { PersonId = 2 } );

    }
}