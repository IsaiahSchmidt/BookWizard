# BookWizard

## API Testing Notes

Using The Fellowship of the Ring by J.R.R Tolkien as an example

https://openlibrary.org/search.json?title=The+Fellowship+Of+The+Rings&author=Tolkien&language=eng

Breaking down the Link Above, 
    title is the title of the book
    author is just a portion of the Authors name
    langauge=eng Forces the books to be english

Once we do this we will get something that looks similar to this 

```json 
{
  "numFound": 5,
  "start": 0,
  "numFoundExact": true,
  "docs": [
    {
      "key": "/works/OL14933414W",
      "type": "work",
      "seed": [
        "/books/OL8889789M",
        "/books/OL22266040M",
        ....
    }
  ]
  ....
}
```

if we then use the Docs.Key information ("/works/OL14933414W").

The generate a the works link
https://openlibrary.org/works/OL14933414W.json

we will get what you see below and which is all the Magic information we need in order to get the book information for our database

```json 
{
  "description": "One Ring to rule them all, One Ring to find them, One Ring to bring them all and in the darkness bind them.\r\n\r\n“A unique, wholly realized other world, evoked from deep in the well of Time, massively detailed, absorbingly entertaining, profound in meaning.”—The New York Times\r\n\r\nIn ancient times the Rings of Power were crafted by the Elven-smiths, and Sauron, the Dark Lord, forged the One Ring, filling it with his own power so that he could rule all others. But the One Ring was taken from him, and though he sought it throughout Middle-earth, it remained lost to him. After many ages it fell into the hands of Bilbo Baggins, as told in The Hobbit. In a sleepy village in the Shire, young Frodo Baggins finds himself faced with an immense task, as his elderly cousin Bilbo entrusts the Ring to his care. Frodo must leave his home and make a perilous journey across Middle-earth to the Cracks of Doom, there to destroy the Ring and foil the Dark Lord in his evil purpose.",
  "links": [
    {
      "title": "Tolkien Estate | The Lord of the Rings",
      "url": "http://www.tolkienestate.com/en/writing/tales-of-middle-earth/the-lord-of-the-rings.html",
      "type": {
        "key": "/type/link"
      }
    },
    {
      "title": "The Fellowship of the Ring - Wikipedia",
      "url": "https://en.wikipedia.org/wiki/The_Fellowship_of_the_Ring",
      "type": {
        "key": "/type/link"
      }
    },
    {
      "title": "The Hero is a Hobbit (New York Times Books)",
      "url": "https://archive.nytimes.com/www.nytimes.com/books/01/02/11/specials/tolkien-fellowship.html",
      "type": {
        "key": "/type/link"
      }
    }
  ],
  "title": "The Fellowship of the Ring",
  "covers": [
    8474036,
    9293479,
    10078899,
    10185087,
    -1,
    10290015,
    10290885,
    9881510,
    10627914,
    11387270,
    10846319,
    10877334,
    10703594,
    10703595,
    11967033,
    1023228
  ],
  "subject_places": [
    "Middle Earth",
    "Mordor",
    "Tierra Media",
    "Bree",
    "Gondor"
  ],
  "subjects": [
    "Elves",
    "Dwarves",
    "evil",
    "fear",
    "hope",
    "young adult fiction",
    "Ficción",
    "Ficción fantástica inglesa",
    "Fantastic fiction",
    "The Lord of the Rings",
    "English Fantasy fiction",
    "Open Library Staff Picks",
    "Fiction",
    "Fantasy",
    "Fairy tales",
    "Adventure stories",
    "Fantasy fiction",
    "Middle Earth (Imaginary place)",
    "History and criticism",
    "FICTION / Fantasy / Epic",
    "Adventure fiction",
    "English fiction",
    "Translations in Chinese",
    "Fiction, fantasy, epic",
    "Middle earth (imaginary place), fiction",
    "Baggins, frodo (fictitious character), fiction",
    "Gandalf (fictitious character), fiction",
    "British and irish fiction (fictional works by one author)",
    "Baggins, bilbo (fictitious character), fiction",
    "Fiction, media tie-in",
    "Children's fiction",
    "English literature",
    "Large type books",
    "Fiction in English",
    "Roman",
    "Français (langue)",
    "Lectures et morceaux choisis",
    "Vol. 1.",
    "Lord of the rings (Tolkien, J.R.R.)",
    "Gandolf (Fictitious character)",
    "Terre du Milieu (Lieu imaginaire)",
    "Romans, nouvelles",
    "Frodo Baggins (Fictitious character)",
    "Fiction, fantasy, general"
  ],
  "subject_people": [
    "Bilbo Baggins",
    "Gandalf the Wizard",
    "Frodo Baggins",
    "Samwise Gamgee",
    "Aragorn (Strider)",
    "Legolas",
    "Gimli",
    "Elrond",
    "Sauron",
    "Black Riders",
    "Tom Bombadil",
    "Boromir",
    "Merry Brandybuck",
    "Celeborn",
    "Galadriel",
    "Elendil",
    "Gandalf"
  ],
  "key": "/works/OL14933414W",
  "authors": [
    {
      "author": {
        "key": "/authors/OL26320A"
      },
      "type": {
        "key": "/type/author_role"
      }
    }
  ],
  "type": {
    "key": "/type/work"
  },
  "excerpts": [
    {
      "excerpt": "This book is largely concerned with Hobbits, and from its pages a reader may discover much of their character and a little of their history.",
      "comment": "first sentence",
      "author": {
        "key": "/people/seabelis"
      }
    }
  ],
  "latest_revision": 53,
  "revision": 53,
  "created": {
    "type": "/type/datetime",
    "value": "2010-03-12T22:23:04.929800"
  },
  "last_modified": {
    "type": "/type/datetime",
    "value": "2023-12-20T18:56:28.521801"
  }
}
```