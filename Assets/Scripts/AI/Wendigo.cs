using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Area;

public enum WendigoState
{
    None,
    Wander,
    Ofensive,
    Sneaky,
    Defensive
}

public class Wendigo : MonoBehaviour
{
    public int max_emotion = 1000;
    private static int mid_emotion = 1001;
    public static int anger = 1001;
    public static int stealth = 1001;
    public static int thirst = 1001;
    public static int health = 1001;

    public float state_count = 0.0f;
    public WendigoState state = WendigoState.Wander;

    public Vector3 wander_target;

    public Area area;

    bool player_seen = false;

    // Start is called before the first frame update
    void Start()
    {
        if(mid_emotion > max_emotion)
            mid_emotion = max_emotion / 2;
        if(anger > max_emotion)
            anger = mid_emotion;
        if(stealth > max_emotion)
            stealth = mid_emotion;
        if(thirst > max_emotion)
            thirst = mid_emotion;
        if(health > max_emotion)
            health = max_emotion;

        if(area == null)
            return;

        this.update(ref anger, area.wendigo_anger);
        this.update(ref stealth, area.wendigo_stealth);
        this.update(ref thirst, area.wendigo_thirst);
        this.update(ref health, area.wendigo_health);
    }

    // Update is called once per frame
    void Update()
    {
        state_count += Time.deltaTime;
        if(state_count >= 1.0f)
        {
            state_count = 0.0f;
            this.state = WendigoState.None;
        }
        switch (state){
            case WendigoState.Wander:
                //if(this.wander_target == Vector3.zero)
                    //his.wander_target = this.area.get_random_position();
                
                break;
            case WendigoState.Ofensive:
                // Select the mid point of the area, or the last point where the player was
                // seen. Go there, and start searching
                // Go fast, and faster if the player is seen
                break;
            case WendigoState.Sneaky:
                // Select a wander position, or anything that catches the attention
                // of the wendigo, and try going there, moving from cover to cover
                // (selected via raycast)
                // Go rather slow.
                break;
            case WendigoState.Defensive:
                // Go slowly, but carefully, and stay in the area

                break;
            default:
                if(very_angry || (angry && careless)){
                    this.state = WendigoState.Ofensive;
                }
                else if(very_stealthy || (stealthy && fine)){
                    this.state = WendigoState.Sneaky;
                }
                else if(hurt || (scratched && (neutral || fine))){
                    this.state = WendigoState.Defensive;
                }
                else{
                    this.state = WendigoState.Wander;
                }
                break;
        }
    }

    public void add_stimulus(float set_anger, float set_stealth, float set_thirst, float set_health)
    {
        this.update(ref anger, set_anger);
        this.update(ref stealth, set_stealth);
        this.update(ref thirst, set_thirst);
        this.update(ref health, set_health);
    }

    void update(ref int emotion, float emotion_value){
        emotion += (int)(
                (mid_emotion - Math.Abs(mid_emotion - emotion)) * emotion_value);
    }

    // EASY USE DEFINITIONS
    private float high_value = 0.85f;
    private float mid_value = 0.65f;
    private float low_value = 0.35f;

    private bool very_angry{
        get{
            return anger > max_emotion * high_value;
        }
    }
    private bool angry{
        get{
            return anger > max_emotion * mid_value;
        }
    }
    private bool neutral{
        get{
            return anger > max_emotion * low_value && anger < max_emotion * mid_value;
        }
    }
    private bool fine{
        get{
            return anger < max_emotion * low_value;
        }
    }

    private bool very_stealthy{
        get{
            return stealth > max_emotion * high_value;
        }
    }
    private bool stealthy{
        get{
            return stealth > max_emotion * mid_value;
        }
    }
    private bool visible{
        get{
            return stealth > max_emotion * low_value && stealth < max_emotion * mid_value;
        }
    }
    private bool careless{
        get{
            return stealth < max_emotion * low_value;
        }
    }

    private bool very_thirsty{
        get{
            return thirst > max_emotion * high_value;
        }
    }
    private bool thirsty{
        get{
            return thirst > max_emotion * mid_value;
        }
    }
    private bool satisfied{
        get{
            return thirst > max_emotion * low_value && thirst < max_emotion * mid_value;
        }
    }

    private bool full_health{
        get{
            return health > max_emotion * high_value;
        }
    }
    private bool healthy{
        get{
            return health > max_emotion * mid_value;
        }
    }
    private bool scratched{
        get{
            return health > max_emotion * low_value && health < max_emotion * mid_value;
        }
    }
    private bool hurt{
        get{
            return health < max_emotion * low_value;
        }
    }
}
