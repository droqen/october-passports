using UnityEngine;
using System.Collections;

namespace passport.story3
{
    
    public interface IStorydecoder
    {
        Story Decode(Pages pages);
    }

}