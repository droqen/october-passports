using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using passport.link;

public class F2Client : MonoBehaviour
{

    public FiefsXXI xxi;

    public bool IsConnected { get { return link.IsConnected; } }

    ClientsideLink link;
    private void Awake()
    {
        link = GetComponent<ClientsideLink>();
    }

    public void AttemptConnection()
    {
        link.AttemptConnection(success => {
            xxi.RefreshClientView();
        });
    }

}
