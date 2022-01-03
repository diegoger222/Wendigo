using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Area;

public enum WendigoState
{
    None=0,
    Wander=1,
    Ofensive=2,
    Sneaky=3,
    Defensive=4
}

// 36,-12
public class Wendigo : MonoBehaviour
{
    public const int max_emotion = 1000;
    private static int mid_emotion = 1001;
    private const int number_emotions = 4;
    public static int anger = 1001;
    public static int stealth = 1001;
    public static int thirst = 0;
    public static int health = 1000;


    [SerializeField]
    public const float speed = 8.0f;
    [SerializeField]
    public const float rotation_speed = 1000.0f;
    [SerializeField]
    public const float attack_speed = 1.0f;
    [SerializeField]
    public const float attack_range = 6f;
    [SerializeField]
    public const float attack_cast_time = 1.25f;
    [SerializeField]
    public const float vision_range = 30.0f;
    [SerializeField]
    public const float vision_angle = 40.0f;
    [SerializeField]
    public const float attack_damage = 45.0f;
    [SerializeField]
    public const float search_radius = 80.0f;
    [SerializeField]
    public const int search_effort = 2;
    [SerializeField]
    public const float emotion_apply_seconds = 10.0f;
    [SerializeField]
    public const float emotion_decay = 0.01f;

    static float _speed = 1.0f;
    static float _rotation_speed = 1.0f;
    static float _att_speed = 1.0f;
    static float _att_range = 1.0f;
    static float _vision_range = 30.0f;
    static float _vision_angle = 40.0f;
    static float _att_damage = 1.0f;
    static float _search_radius = 1.0f;
    static int _search_effort = 2;
    static int _searched = 0;

    //
    static float eye_separation = 1.0f;

    public static float stat_modification_step = 0.35f;

    public static WendigoState state = WendigoState.None;

    public Vector3 wander_target;
    public Vector3 player_target;

    public Area area;
    static Animator animator;

    bool player_need_check = true;
    static bool player_seen = false;
    bool player_was_seen = false;
    bool player_in_range = false;
    bool player_sneaky_chase = false;

    bool wander_target_set = false;

    bool can_attack = true;
    bool stunned = false;
    bool can_sprint = true;

    [Header("Stay still until player is seen?")]
    public bool look_around = false; // Set to true for the Wendigo to stay still
    Vector3 look_around_target;
    float pre_sprint_speed = 0.0f;
    float sneaky_distance_threshold = 0.0f;
    float dist = 0.0f;
    bool sneaky_cover_avaliable = false;
    bool cover_need_check = true;
    Vector3 sneaky_cover_position;


    public static GameObject player;
    private static UnityEngine.AI.NavMeshAgent agent;


    bool __logging = false;

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

        if(agent == null){
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            
            agent.stoppingDistance = (Wendigo.attack_range - 0.1f)/10;
            agent.autoRepath = true;
        }
        if(area != null){
            this.update(ref anger, area.wendigo_anger);
            this.update(ref stealth, area.wendigo_stealth);
            this.update(ref thirst, area.wendigo_thirst);
            this.update(ref health, area.wendigo_health);
        }
        if(animator == null){
            animator = GetComponent<Animator>();

            if(!look_around)
                animator.SetBool("Andar", true);
        }
        if(player == null){
            player = GameObject.FindGameObjectWithTag("Player");
        }

        InvokeRepeating("time_stimulus", 1.0f, Wendigo.emotion_apply_seconds);
    }

    void log_distance_th(){
        Debug.Log("Waiting for distance threshold: " + dist + "/" + sneaky_distance_threshold);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(player_need_check){
            player_need_check = false;
            Invoke("check_player", player_seen ? 0.75f : 0.25f);
        }
        if(stunned){
            transform.LookAt(player.transform);
            return;
        }

        if(look_around){
            // Check if we are looking at look_around_target
            if(Vector3.Angle(transform.forward, look_around_target - transform.position) < Wendigo.vision_angle/2){
                // Generate a new target
                look_around_target = new Vector3(UnityEngine.Random.Range(-Wendigo.vision_range, Wendigo.vision_range), 1.0f, UnityEngine.Random.Range(-Wendigo.vision_range, Wendigo.vision_range));
            }

            // Look at the target
            transform.LookAt(look_around_target);

            if(player_seen)
                look_around = false;
            else 
                return;
        }


        switch (state){
            case WendigoState.Wander:
                if(player_seen){
                    //Debug.Log("Player seen");

                    // recalculate dist to have a more accurate distance
                    float att_distance = Vector3.Distance(transform.position, player.transform.position);

                    // Attack the player if we are in range, and we can attack
                    if(can_attack && att_distance <= Wendigo._att_range){
                        Debug.Log("Attack");
                        this.can_attack = false;

                        animator.SetBool("Atacar", true);
                        animator.SetBool("Andar", false);

                        agent.ResetPath();

                        Invoke("damage_player", Wendigo.attack_cast_time);
                        Invoke("attack_cooldown", 2/Wendigo.attack_speed);
                    }
                    else if(!agent.pathPending && att_distance >= Wendigo._att_range*1.1f){
                        Debug.Log("Chase player");

                        animator.SetBool("Atacar", false);
                        animator.SetBool("Andar", true);

                        agent.SetDestination(player.transform.position);
                        
                    } else if(animator.GetBool("Andar")){
                        agent.ResetPath();
                        animator.SetBool("Andar", false);
                    } else {
                        transform.LookAt(player.transform.position);
                    }
                } else if(! agent.pathPending && !agent.hasPath){
                    if(player_was_seen){
                        search_player();
                    } else {
                        select_wander_target();
                    } 
                } else if(! agent.pathPending && agent.remainingDistance <= agent.stoppingDistance){
                    Debug.Log("Wander target reached. Distance: " + agent.remainingDistance + "/" + agent.stoppingDistance);

                    wander_target_set = false;
                }
                        
                
                break;
            case WendigoState.Ofensive:
                goto case WendigoState.Wander;
                break;
            case WendigoState.Sneaky:
                // Select a wander position, or anything that catches the attention
                // of the wendigo, and try going there, moving from cover to cover
                // Go rather slow.
                if(player_seen){
                    // Either wait until it moves
                    // or
                    if(dist < _vision_range/4){
                        player_sneaky_chase = true;
                        if(Debug.isDebugBuild){
                            CancelInvoke("log_distance_th");
                            __logging = false;
                        }
                    }

                    if(!player_sneaky_chase && dist < _vision_range/2){
                        if(sneaky_distance_threshold == 0.0f){
                            Debug.Log("Player seen. Waiting for it to move");
                            agent.ResetPath();

                            animator.SetBool("Andar", false);

                            sneaky_distance_threshold = dist+1.0f;
                        } 
                        else if(dist > sneaky_distance_threshold){
                            player_sneaky_chase = true;
                            animator.SetBool("Andar", true);

                            if(Debug.isDebugBuild){
                                CancelInvoke("log_distance_th");
                                __logging = false;
                            }
                        } else {
                            if(!__logging && Debug.isDebugBuild){
                                InvokeRepeating("log_distance_th", 1.0f, 1.0f);
                                __logging = true;
                            }

                            transform.LookAt(player.transform.position);
                        }
                    }
                    else
                        goto case WendigoState.Wander;
                } else {
                    if(cover_need_check && !sneaky_cover_avaliable){
                        Debug.Log("Checking for a cover");
                        cover_need_check = false;
                        Invoke("select_sneaky_cover", 2.0f);
                    }

                    if(! agent.pathPending && !agent.hasPath)
                        if(player_was_seen)
                            // Tha player was seen
                            search_player();
                        else if(wander_target_set && !agent.isPathStale)
                            // We have a destination, but we went to a cover
                            agent.SetDestination(wander_target);
                        else
                            // We reached the destination
                            select_wander_target(false);
                    else if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance){
                        Debug.Log("Wander target reached. Distance: " + agent.remainingDistance + "/" + agent.stoppingDistance);

                        wander_target_set = false;
                    }
                    
                }
                break;
            case WendigoState.Defensive:
                // Go slowly, but carefully, and stay in the area
                if(player_seen && can_sprint){
                    Debug.Log("Wendigo is sprinting");
 
                    can_sprint = false;
                    pre_sprint_speed = agent.speed;
                    agent.speed = agent.speed * 2f;

                    Invoke("deactivate_sprint", 3.5f);
                    Invoke("sprint_cooldown", 20.0f);
                }

                goto case WendigoState.Wander;
                break;
            default:
                state = get_state();

                this.set_stats();
                break;
        }
    }

    public void check_player(){
        // Check if the player is in range.
        // If so, check if the player is visible.

        dist = Vector3.Distance(this.transform.position, player.transform.position);
        //Debug.Log("Player distance: " + dist);
        player_need_check = true;

        if(dist <= _vision_range){
            //Debug.Log("Player in range");
            this.player_in_range = true;

            if(dist < _vision_range / 3){
                //Debug.Log("Player in vision range");
                detect_player();

                return;
            }

            // Check if the player is in the vision angle, or close enough
            else if(Math.Abs(Vector3.Angle(player.transform.position - this.transform.position, 
                                        this.transform.forward)) <= _vision_angle){
                // Raycast to see if the player is visible from any of the
                // wendigo's eyes, separated by this.eye_separation
                // on the local z axis.
                RaycastHit hit;

                //Debug.Log("To be seen");

                if((Physics.Raycast(this.transform.position + new Vector3(0, 1, -eye_separation), player.transform.position - this.transform.position, out hit, _vision_range)
                            && hit.transform.gameObject.tag == "Player")
                            || (Physics.Raycast(this.transform.position + new Vector3(0, 1, eye_separation), 
                                                player.transform.position - this.transform.position, out hit, _vision_range)
                                && hit.transform.gameObject.tag == "Player")){
                    

                    //Debug.Log("Player seen (enable)");
                    
                    detect_player();

                    return;
                } else {
                    //Debug.Log("Player not seen in raycast");
                }
            } else {
                //Debug.Log("Player not seen. Angle " + Vector3.Angle(player.transform.position - this.transform.position, 
                                        //this.transform.forward));
            }
        } else {
            //Debug.Log("Player not in range");

            this.player_in_range = false;
        }

        if(player_seen){
            Debug.Log("Player was seen, but it is not anymore");
            // If the player was seen, store the position of the player
            // from where to search.
            this.player_target = player.transform.position;
            this.player_was_seen = true;

            this.look_around = true;
            Invoke("end_look_around", _search_effort);
        }

        player_seen = false;
    }

    public static void detect_player(bool set_destination = false){
        if(set_destination || (! player_seen && state != WendigoState.Sneaky)){
            agent.SetDestination(player.transform.position);
        }
        player_seen = true;
    }

    void search_player(){

        // Search for the player, _search_effort times.
        // Look around as much as possible, and wander around
        // the last known player position, player_target.

        if(Wendigo._searched < Wendigo._search_effort){
            ++Wendigo._searched;
            Debug.Log("Searching player " + Wendigo._searched + "/" + Wendigo._search_effort);

            // Get a random point in the area of player_target, 
            // with a radius of _search_radius
            // Move to the search point
            agent.SetDestination(RandomPoints.random_point_circle(this.player_target, Wendigo._search_radius));


            if(! agent.pathPending && agent.hasPath){
                ++Wendigo._searched;
            }
        } else {
            Debug.Log("Search ended");

            Wendigo._searched = 0;
            player_was_seen = false;
            player_sneaky_chase = false;
            sneaky_distance_threshold = 0.0f;
        }
    }

    void select_wander_target(bool check = true){
        Debug.Log("Selecting wander target");
        if(!check || ! wander_target_set || agent.isPathStale ){
            if(area == null || Wendigo.thirst > UnityEngine.Random.Range(0, Wendigo.max_emotion)){
                wander_target = RandomPoints.random_point();
            } else {
                wander_target = area.get_random_point();
            }
            wander_target_set = true;

            agent.SetDestination(wander_target);
        }
    }

    void select_sneaky_cover(){
        UnityEngine.AI.NavMeshHit hit;
        agent.FindClosestEdge(out hit);
        cover_need_check = true;

        // Check that the point is in the direction of the wander target
        if(Vector3.Distance(hit.position, sneaky_cover_position) > 5.0f)
            // Check the angle against the forward vector, because not always the path is straight
            if(Math.Abs(Vector3.Angle(hit.position - this.transform.position, this.transform.forward)) < _vision_angle/2){
                Debug.Log("Got a cover in " + hit.position);

                this.sneaky_cover_position = hit.position;
                this.sneaky_cover_avaliable = true;

                agent.SetDestination(hit.position);

                return;
            }

        Debug.Log("No good cover found");
        sneaky_cover_avaliable = false;
    }

    private void time_stimulus(){
        WendigoState new_state = get_state();
        if(player_seen){
            // If the player is seen, there is no need to wander.
            if(new_state != WendigoState.Wander && new_state != state){
                state = new_state;
                this.set_stats();

                Debug.Log("State: " + state);  
            }

            Debug.Log("Wendigo gets angrier by the player's presence");

            // Get angrier by the player's presence
            this.update(ref anger, 0.1f);
            Debug.Log("Anger: " + anger*100/Wendigo.max_emotion + 
                "% Stealth: " + stealth*100/Wendigo.max_emotion + 
                "% Thirst: " + thirst*100/Wendigo.max_emotion + 
                "% Health: " + health*100/Wendigo.max_emotion + "%");
        } else {
            if(new_state != state){
                state = new_state;
                this.set_stats();

                Debug.Log("State: " + state);
            }

            Debug.Log("Wendigo gets more thirsty by the player's absence");

            // Get more thirsty by the player's absence
            this.add_stimulus(-Wendigo.emotion_decay, -Wendigo.emotion_decay, 0.1f, Wendigo.emotion_decay);
        }
    }

    void damage_player(){
        if(Vector3.Distance(transform.position, player.transform.position) <= _att_range)
            GameObject.Find("Player").GetComponent<BarraDeVida>().RestarVida(_att_damage);
    }

    public void receive_damage(float dmg, float stun_duration=1.5f){
        Debug.Log("Wendigo received damage");

        this.stunned = true;
        this.update(ref health, -dmg/100);
        this.update(ref anger, dmg/200);

        animator.enabled = false;
        animator.SetBool("Andar", false);
        animator.SetBool("Atacar", false);
        animator.enabled = true;
        animator.SetBool("Gritar", true);

        if(state == WendigoState.Sneaky){
            player_sneaky_chase = true;
        }
        agent.ResetPath();

        CancelInvoke("damage_player");
        Invoke("deactivate_stun", stun_duration);

        CancelInvoke("time_stimulus");
        time_stimulus();
    }

    public void add_stimulus(float set_anger, float set_stealth, float set_thirst, float set_health)
    {
        this.update(ref anger, set_anger);
        this.update(ref stealth, set_stealth);
        this.update(ref thirst, set_thirst);
        this.update(ref health, set_health);

        Debug.Log("Anger: " + anger*100/Wendigo.max_emotion + 
                "% Stealth: " + stealth*100/Wendigo.max_emotion + 
                "% Thirst: " + thirst*100/Wendigo.max_emotion + 
                "% Health: " + health*100/Wendigo.max_emotion + "%");
    }

    void update(ref int emotion, float emotion_value){
        emotion += (int)(
                (mid_emotion - Math.Abs(mid_emotion - emotion)) * emotion_value);
    }

    int _argmax_state(ref int max_emotion){
        int max_state = (int) WendigoState.None;
        if(anger < max_emotion){
            max_emotion = anger;
            goto already_modified_first;
        } else if(stealth < max_emotion){
            max_emotion = stealth;
            goto already_modified;
        } else if(thirst < max_emotion){
            max_emotion = thirst;
            goto already_modified;
        } else if(health < max_emotion){
            max_emotion = health;
            goto already_modified;
        }
        return max_state;

        already_modified:

        if(anger > max_emotion){
            max_emotion = anger;
            max_state = (int) WendigoState.Ofensive;
        }
        already_modified_first:
        if(stealth > max_emotion){
            max_emotion = stealth;
            max_state = (int) WendigoState.Sneaky;
        }
        if(thirst > max_emotion){
            max_emotion = thirst;
            max_state = (int) WendigoState.Wander;
        }
        if(max_emotion-health > max_emotion){
            max_emotion = max_emotion-health;
            max_state = (int) WendigoState.Defensive;
        }

        return max_state;
    }

    WendigoState get_state(){
        int emotion_value = max_emotion+1;

        while(true){
            // Get the highest value of the emotions
            // and remove it from the dictionary.

            switch(_argmax_state(ref emotion_value)){
                case (int)WendigoState.Ofensive:
                    if(very_angry || (angry_or_higher && (visible_or_lower || very_thirsty))){
                        return WendigoState.Ofensive;
                    }
                    break;
                case (int)WendigoState.Sneaky:
                    if(very_stealthy || (stealthy_or_higher && fine)){
                        cover_need_check = true;
                        if(state != WendigoState.Sneaky)
                            player_sneaky_chase = false;
                        return WendigoState.Sneaky;
                    }
                    break;
            
                case (int)WendigoState.Defensive:
                    if(hurt || (scratched && neutral_or_lower)){
                        return WendigoState.Defensive;
                    }
                    break;
                case (int)WendigoState.Wander:
                    continue;
                default:
                    return WendigoState.Wander;
            }
        }
            
        return WendigoState.Wander;
    }

    void set_stats(){
        Debug.Log("Setting stats");
        switch(state){
            case WendigoState.Wander:
            set_stats_wander:
                //_speed = speed;
                agent.speed = speed;
                agent.angularSpeed = rotation_speed;
                _att_speed = attack_speed;
                _att_range = attack_range;
                _vision_range = vision_range;
                _vision_angle = vision_angle;
                _att_damage = attack_damage;
                _search_radius = search_radius;
                _search_effort = search_effort;
                break;
            case WendigoState.Ofensive:
                agent.speed = modifier(speed);
                agent.angularSpeed = rotation_speed;
                _att_speed = modifier(attack_speed);
                _att_range = modifier(attack_range, -1.0f);
                _vision_range = vision_range;
                _vision_angle = modifier(vision_angle, -1.0f);
                _att_damage = modifier(attack_damage);
                _search_radius = modifier(search_radius, -0.5f);
                _search_effort = modifier(search_effort);
                break;
            case WendigoState.Sneaky:
                agent.speed = modifier(speed, -1.0f);
                agent.angularSpeed = modifier(rotation_speed, 1.5f);
                _att_speed = modifier(attack_speed);
                _att_range = modifier(attack_range);
                _vision_range = modifier(vision_range);
                _vision_angle = modifier(vision_angle);
                _att_damage = modifier(attack_damage, -1.0f);
                _search_radius = modifier(search_radius);
                _search_effort = search_effort;
                break;
            case WendigoState.Defensive:
                agent.speed = modifier(speed, -1.5f);
                agent.angularSpeed = rotation_speed;
                _att_speed = modifier(attack_speed, -1.0f);
                _att_range = attack_range;
                _vision_range = vision_range;
                _vision_angle = modifier(vision_angle);
                _att_damage = modifier(attack_damage, 2f);
                _search_radius = search_radius;
                _search_effort = search_effort;
                break;

            default:
                goto set_stats_wander;
                break;
        }

        Debug.Log("Set stats" + 
            "\nSpeed: " + agent.speed + 
            "\nRotation speed: " + agent.angularSpeed + 
            "\nAttack speed: " + _att_speed + 
            "\nAttack range: " + _att_range + 
            "\nVision range: " + _vision_range + 
            "\nVision angle: " + _vision_angle + 
            "\nAttack damage: " + _att_damage + 
            "\nSearch radius: " + _search_radius + 
            "\nSearch effort: " + _search_effort);  
    }

    float modifier(float def_value, float times=1.0f){
        return def_value + def_value * (times * Wendigo.stat_modification_step);
    }

    int modifier(int def_value, float times=1.0f){
        return (int)(def_value + def_value * (times * Wendigo.stat_modification_step));
    }

    // Abilities
        void attack_cooldown(){
            this.can_attack = true;
        }

        void sprint_cooldown(){
            this.can_sprint = true;
        }

        void deactivate_look_around(){
            this.look_around = false;
        }

        void deactivate_sprint(){
            Wendigo._speed = this.pre_sprint_speed;
        }

        void deactivate_stun(){
            this.stunned = false;
            animator.enabled = true;
        }

        void end_look_around(){
            this.look_around = false;
        }

    // EASY USE DEFINITIONS
        // Remove const only if needed
        private const float high_value = 0.85f;
        private const float mid_value = 0.65f;
        private const float low_value = 0.35f;

        private bool very_angry{
            get{
                return anger > max_emotion * high_value;
            }
        }

        private bool angry{
            get{
                return anger > max_emotion * mid_value && anger < max_emotion * high_value;
            }
        }
        private bool angry_or_higher{
            get{
                return anger > max_emotion * mid_value;
            }
        }
        private bool angry_or_lower{
            get{
                return anger < max_emotion * high_value;
            }
        }

        private bool neutral{
            get{
                return anger > max_emotion * low_value && anger < max_emotion * mid_value;
            }
        }
        private bool neutral_or_higher{
            get{
                return anger > max_emotion * low_value;
            }
        }
        private bool neutral_or_lower{
            get{
                return anger < max_emotion * mid_value;
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
                return stealth > max_emotion * mid_value && stealth < max_emotion * high_value;
            }
        }
        private bool stealthy_or_higher{
            get{
                return stealth > max_emotion * mid_value;
            }
        }
        private bool stealthy_or_lower{
            get{
                return stealth < max_emotion * high_value;
            }
        }

        private bool visible{
            get{
                return stealth > max_emotion * low_value && stealth < max_emotion * mid_value;
            }
        }
        private bool visible_or_higher{
            get{
                return stealth > max_emotion * low_value;
            }
        }
        private bool visible_or_lower{
            get{
                return stealth < max_emotion * mid_value;
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
                return thirst > max_emotion * mid_value && thirst < max_emotion * high_value;
            }
        }
        private bool thirsty_or_higher{
            get{
                return thirst > max_emotion * mid_value;
            }
        }
        private bool thirsty_or_lower{
            get{
                return thirst < max_emotion * high_value;
            }
        }

        private bool satisfied{
            get{
                return thirst > max_emotion * low_value && thirst < max_emotion * mid_value;
            }
        }
        private bool satisfied_or_higher{
            get{
                return thirst > max_emotion * low_value;
            }
        }
        private bool satisfied_or_lower{
            get{
                return thirst < max_emotion * mid_value;
            }
        }


        private bool full_health{
            get{
                return health > max_emotion * high_value;
            }
        }

        private bool healthy{
            get{
                return health > max_emotion * mid_value && health < max_emotion * high_value;
            }
        }
        private bool healthy_or_higher{
            get{
                return health > max_emotion * mid_value;
            }
        }
        private bool healthy_or_lower{
            get{
                return health < max_emotion * high_value;
            }
        }

        private bool scratched{
            get{
                return health > max_emotion * low_value && health < max_emotion * mid_value;
            }
        }
        private bool scratched_or_higher{
            get{
                return health > max_emotion * low_value;
            }
        }
        private bool scratched_or_lower{
            get{
                return health < max_emotion * mid_value;
            }
        }

        private bool hurt{
            get{
                return health < max_emotion * low_value;
            }
        }
}
