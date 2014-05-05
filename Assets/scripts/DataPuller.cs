using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;
//using System.Xml.Linq;

public class Person
{

    public string active;
    public string hero;
    public string familyName;
    public string givenName;
    public string location;
    public string lifespan;
    public string filename;
    public string filepath;
    public string description;
    public string id;
}
public class DataPuller : MonoBehaviour
{
    public XmlDocument data;
    public List<XmlNodeList> myList;
    public IEnumerable<Person> heroes;
    List<Person> people;
    public static List<Person> herosList;
    public static List<Person> normalPeople;
    //I set this up as a delegate so more than one function could subscribe to it if need be... 
    public delegate void SetData();
    public SetData dataItem;

    //this one sets the data item to get 
    static public int num = 0;
    //this vars only job is to act at a holder for the current node you want to pull out 
    static public Person currentHero;

    public static List<Person> activeHeroes;
    public static List<Person> inactiveHeroes;
    public static int SetNumHeroPeople = 2;

    public static List<Person> activeNormalPeople;
    public static List<Person> inactiveNormalPeople;
    public static int SetNumNormalPeople = 6;

    void Start()
    {


        //set up the deligate to set a current hero. 
        dataItem += PickHeroData;

        people = new List<Person>();
        //load up the data 
        data = new XmlDocument();
        data.Load("./Assets/data/data.xml");

        XmlElement elm = data.DocumentElement;
        XmlNodeList nodeData = elm.ChildNodes;

        //this will get random nodes 
        Debug.Log(nodeData.Count + " items in db");

        inactiveNormalPeople = new List<Person>();
        activeNormalPeople = new List<Person>();
        activeHeroes = new List<Person>();
        inactiveHeroes = new List<Person>();


        for (int i = 0; i < nodeData.Count; i++)
        {
            if (nodeData[i].Name == "ROW")
            {
                Person p = new Person();
                p.familyName = nodeData[i][
                                           "FamilyName"].InnerText;
                p.givenName = nodeData[i]["GivenName"].InnerText;
                p.location = nodeData[i]["Location"].InnerText;
                p.hero = nodeData[i]["Hero"].InnerText;
                p.active = nodeData[i]["Active"].InnerText;
                p.lifespan = nodeData[i]["Lifespan"].InnerText;
                p.filename = nodeData[i]["Filename"].InnerText;
                p.filepath = nodeData[i]["File_Path"].InnerText;
                p.description = nodeData[i]["HeroDescription"].InnerText;
                p.id = nodeData[i]["UID"].InnerText;
                people.Add(p);

            }
        }
        foreach (Person p in people)
        {
            //			Debug.Log( p.familyName + " is in the database"); 
        }
        //if not hero toss in static not hero list?

        Debug.Log("the database has " + people.Count + " records ");
        GetHeros();

        //		foreach(Person p in normalPeople)
        //		{
        //			Debug.Log(p.givenName + p.familyName);
        //		}
        GetNormalPeople();
        SetActiveHeroes();
        SetActiveNormalPeople();
        Debug.Log("There are " + activeHeroes.Count() + " active heroes");
        Debug.Log("There are " + activeNormalPeople.Count() + " active normal people");

        shuffleList(activeNormalPeople);
        shuffleList(activeHeroes);

        //working remove test 
        RemoveHeroFromActiveList(activeHeroes[0]);
        RemoveNormalPersonFromActiveList(activeNormalPeople[0]);
        Debug.Log("There are " + activeHeroes.Count() + " active heroes after remove");
        Debug.Log("There are " + activeNormalPeople.Count() + " active normal people after remove");

        Person pn = PullNewNormalPerson();
        Debug.Log(pn.familyName + "the pulled normal person");
        Person pa = PullNewHero();
        Debug.Log(pa.familyName + "the pulled hero");

        RemoveHeroFromActiveList(pa);
        RemoveNormalPersonFromActiveList(pn);

        Person pn1 = PullNewNormalPerson();
        Debug.Log(pn1.familyName + "the pulled normal person double test");
        Person pa1 = PullNewHero();
        Debug.Log(pa1.familyName + "the pulled hero double test");

    }
    //this function should set up the initial inactive and active hero lists  
    public static void SetActiveHeroes()
    {
        //add in an active check. 

        for (int i = 0; i < SetNumHeroPeople; i++)
        {
            Person temp = herosList[i];
            activeHeroes.Add(temp);
        }
        int numNotUsed = (int)herosList.Count() - SetNumHeroPeople;
        Debug.Log(numNotUsed + " num of hereos not used.");
        for (int i = herosList.Count() - numNotUsed; i < herosList.Count(); i++)
        {
            inactiveHeroes.Add(herosList[i]);
        }
        Debug.Log("there are " + inactiveHeroes.Count() + " innactive heroes.");
    }

    //this function lets you remove a hero from the active list and add them to the inactive one
    public static void RemoveHeroFromActiveList(Person p)
    {
        for (int i = 0; i < activeHeroes.Count; i++)
        {
            if (p.id == activeHeroes[i].id)
            {
                inactiveHeroes.Add(p);
                activeHeroes.RemoveAt(i);
            }
        }
    }

    //this function lets you get a new hero out of the innactive list and put it to the active list
    public static Person PullNewHero()
    {
        activeHeroes.Add(inactiveHeroes[0]);
        //always get the first one in the list 
        Person temp = inactiveHeroes[0];
        inactiveHeroes.RemoveAt(0);

        return temp;
    }
    //sets up the inactive and active normal people lists 
    public static void SetActiveNormalPeople()
    {

        if (SetNumNormalPeople > normalPeople.Count())
        {
            Debug.Log("invalid num of normal people. Please try again");
            return;
        }

        for (int i = 0; i < SetNumNormalPeople; i++)
        {
            Person temp = normalPeople[i];
            activeNormalPeople.Add(temp);
        }
        int numNotUsed = (int)normalPeople.Count() - SetNumNormalPeople;
        for (int i = normalPeople.Count() - numNotUsed; i < normalPeople.Count(); i++)
        {
            inactiveNormalPeople.Add(normalPeople[i]);
        }
        Debug.Log("there are " + inactiveNormalPeople.Count() + "inactive Normal people");
    }
    //lets you remove a normal person from the current normal active list and put them in the normal inactive list 
    public static void RemoveNormalPersonFromActiveList(Person p)
    {
        for (int i = 0; i < activeNormalPeople.Count(); i++)
        {
            if (p.id == activeNormalPeople[i].id)
            {
                inactiveNormalPeople.Add(p);
                activeNormalPeople.RemoveAt(i);
                Debug.Log("Person removed " + p.familyName);
            }
        }
    }
    //gets a new normal person out of the inactive list and adds them to the active list. 
    public static Person PullNewNormalPerson()
    {
        activeNormalPeople.Add(inactiveHeroes[0]);
        Person temp = inactiveNormalPeople[0];
        inactiveNormalPeople.RemoveAt(0);
        return temp;

    }


    public void GetHeros()
    {
        heroes = from person in people
                 where person.hero == "yes"
                 select person;

        herosList = new List<Person>();
        foreach (Person p in heroes)
        {
            herosList.Add(p);
        }

        int num = herosList.Count;
        Debug.Log("there are " + herosList.Count() + " heroes");
    }

    public void GetNormalPeople()
    {
        heroes = from person in people
                 where person.hero == "no"
                 select person;

        normalPeople = new List<Person>();
        foreach (Person p in heroes)
        {
            normalPeople.Add(p);
        }

        int num = normalPeople.Count();
        Debug.Log("there are " + normalPeople.Count() + " normal people");


    }

    public void PickHeroData()
    {
        //to use. 
        //1 set the static number of the hero you want 
        // 2 call this function 
        // 3 get the current hero
        currentHero = people[0] as Person;
    }

    public void shuffleList(List<Person> l)
    {
        System.Random rng = new System.Random();

        var n = (int)(l.Count());
        while (n > 1)
        {
            int k = (int)rng.Next(n);
            n--;
            Person temp = l[n];
            l[n] = l[k];
            l[k] = temp;
        }


    }
}