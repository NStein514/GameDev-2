using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    protected override void PrimaryFire()
    {
        if (shoot_delay_timer <= 0)
        {
            if (primary_fire_is_shooting || primary_fire_hold)
            {
                shoot_delay_timer = gun_data.primary_fire_delay; //Delay shooting 
                primary_fire_is_shooting = false;

                //Shoot out 6 pellets
                for(int i = 0; i < 6; i++)
                {
                    //Determine Raycast Direction 
                    Vector3 dir = Quaternion.AngleAxis(Random.Range(-gun_data.spread, gun_data.spread), Vector3.up) * cam.transform.forward;
                    dir = Quaternion.AngleAxis(Random.Range(-gun_data.spread, gun_data.spread), Vector3.right) * cam.transform.forward;

                    //Raycast 
                    ray = new Ray(cam.transform.position, dir);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, gun_data.range))
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.green, 0.05f);
                    }

                    //Trails
                    TrailRenderer trail = Instantiate(bullet_trail, shoot_point.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, dir, hit));
                }

                ammo_in_clip--;
                if (ammo_in_clip <= 0) ammo_in_clip = gun_data.ammo_per_clip;

                //Muzzle Flash
                muzzle_flash.Play();
            }
        }
    }
}
