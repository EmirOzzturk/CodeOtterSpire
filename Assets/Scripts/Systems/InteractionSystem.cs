using Action_System;
using UnityEngine;

public class InteractionSystem : Singleton<InteractionSystem>
{
   public bool PlayerIsDragging { get; set; } = false;

   public bool PlayerCanInteract()
   {
      if (!ActionSystem.Instance.IsPreforming) return true;
      return false;
   }

   public bool PlayerCanHover()
   {
      if (PlayerIsDragging) return false;
      return true;
   }
}
