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
}
public class DataPuller : MonoBehaviour
{
    public XmlDocument data;
    public List<XmlNodeList> myList;
    public IEnumerable<Person> heroes;
    List<Person> people;
    List<Person> herosList;

    //I set this up as a delegate so more than one function could subscribe to it if need be... 
    public delegate void SetData();
    public SetData dataItem;
    
    //this one sets the data item to get 
    static public int num=0;
    //this vars only job is to act at a holder for the current node you want to pull out 
    static public Person currentHero; 

  //random nodes -- implement later 
    ArrayList randomNums; 

    void Awake()
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
                people.Add(p);
            }
        }
        foreach (Person p in people)
        {
         //  Debug.Log( p.familyName + " is in the database"); 
        }
        //if not hero toss in static not hero list?

        Debug.Log("the database has " + people.Count + " records ");
        GetHeros();     
    }

    public void GetHeros()
    {
        heroes = from person in people
                                 where person.hero == "yes"
                                 select person;

        herosList = new List<Person>(); 
        foreach(Person p in heroes)
        {
           herosList.Add(p); 
        }
       
        int num = herosList.Count; 
        Debug.Log("there are " + num + " heroes" ); 
    }

    public void PickHeroData()
    {
        //to use. 
        //1 set the static number of the hero you want 
        // 2 call this function 
        // 3 get the current hero
        currentHero = herosList[num] as Person;
    }
}