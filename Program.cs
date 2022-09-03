// See https://aka.ms/new-console-template for more information

bool[,] grid = new bool[8,5];
Emu george = new Emu(3,4, grid);
//Emu frank = new Emu(1, 3, grid);
george.takeTurn();





    
    
    
   //CLASS DECLARATIONS 
    
    //Character Class. Encompasses both Emus and player character.
public class Character
{
    public int x = 0;
    public int y = 0;
    public int id = 0;
    public bool[,] grid = new bool[5,8];

    //Actions are stored in a doubly linked list. List is doubly linked for action deletion purposes.
     Action? queue = null;
     Action? head = null;
    
    //Prototype character constructor.
    Character(int x, int y, int id)
    {
        this.x = x;
        this.y = y;
    }

    //Character constructor for subclasses.
    public Character()
    {

    }

    //Position change functions. Should detect things like collision with other entities.
    public int changeXPos(int dir)
    {
        if(x+dir<8 && x+dir > -1 && !grid[x+dir,y])
        {
            grid[x, y] = false;
            x+=(1*dir);
            grid[x, y] = true;
            return x;
        }
        return x;
    }
    public int changeYPos(int dir){
        if(y+dir<5 && y+dir > -1 && !grid[x,y+dir])
        {
            grid[x, y] = false;
            y+=1*dir;
            grid[x, y] = true;
            return y;
        }
        return y;
    }

    //Adds an action to the queue. 
    public Action addAction(Action a)
    {
        if(queue!=null)
        {
            queue.setNext(a);
            a.setPrev(queue);
        } else {
            head = a;
        }
        queue = a;
        return queue;
    }

    //Executes the first action in the queue, then removes it from the queue.
    public bool execute()
    {
        if(head!=null)
        {
           head.execute();
           head = head.getNext();
           return true; 
        }
        Console.WriteLine("Head was null here");
        return false;

    }

    //Executes the entire queue. Whiz! Bang! Pop! Like that. This probably won't be what gets used in
    //the final game, but it's useful for testing.
    public void go()
    {
        while(head!=null)
        {
            execute();
            if(head == null)
            {
                break;
            }
            
        }
    }
}

//Emu class. Encompasses only basic Emu.
class Emu:Character
{
    //Emu constructor. In theory, including grid here should let all characters see each other on the grid.
    //TODO: Make sure Characters can see each other on grid
    int projX;
    int projY;
    public Emu(int x, int y, bool[,] grid)
    {
        this.x = x;
        this.y = y;
        id = 2;
        this.grid = grid;
        grid[x,y] = true;
    }

    //Emu turn behavior.
    public void takeTurn()
    {
        int moves = 5;
        projX = x;
        projY = y;
        int targX = 1;
        int targY = 1;
        int diffX = targX - projX;
        int diffY = targY - projY;
        int xDir = diffX/Math.Abs(diffX);
        int yDir = diffY/Math.Abs(diffY);
        while(projX!=targX && moves > 1)
        {
            addAction(new Move(xDir, 0, this));
            projX+=xDir;
            moves--;
        }
        while(projY!=targY && moves > 1)
        {
            addAction(new Move(0, yDir, this));
            projY+=yDir;
            moves--;
        }
        if(Math.Abs(diffX)+Math.Abs(diffY)<4)
        {
            while(moves > 0)
            {
                addAction(new Attack(moves));
                moves--;
            }   
        } else {
            addAction(new Dodge(moves));
        }
        go();
    }
}

//Base Action class. Should not be added to any queues.
public abstract class Action
{
    Action? next = null;
    Action? prev = null;
    public abstract bool execute();
    public Action()
    {

    }
    public Action getNext()
    {
        return next;
    }
    public Action setNext(Action a)
    {
        next = a;
        return a;
    }
    public Action setPrev(Action a)
    {
        prev = a;
        return a;
    }
}

//"Attack" action. Currently just a placeholder, but able to be added to queues.
class Attack:Action{
    int id = 0;
    public Attack(int id)
    {
        this.id = id;
    }
    public override bool execute()
    {
        Console.WriteLine("Attack!!" + id);
        return true;
    }
}

class Dodge:Action
{
    int id;
    public Dodge(int id)
    {
        this.id = id;
    }
    public override bool execute()
    {
        Console.Write("Dodge!!! " + id);
        return true;
    }
}
class Move:Action
{
    int x = 0;
    int y = 0;
    Character? target = null;
    public Move(int x, int y, Character target)
    {
        this.x = x;
        this.y = y;
        this.target = target;
    }
    public override bool execute()
    {
        if(target!=null)
        {
            target.changeXPos(x);
            target.changeYPos(y);
            Console.WriteLine("X:" + target.x + "Y:" + target.y);
        }
       return true;
    }
}


