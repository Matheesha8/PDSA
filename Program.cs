using System;
using System.Collections.Generic;

namespace ZooManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Zoo Management System!");
            Zoo zoo = new Zoo();
            bool running = true;
            while (running)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Add Animal");
                Console.WriteLine("2. List Animals by Name");
                Console.WriteLine("3. List Animals by Color");
                Console.WriteLine("4. List Animals by Size");
                Console.WriteLine("5. List Animals by Weight");
                Console.WriteLine("6. Remove Animal");
                Console.WriteLine("7. Search Animal");
                Console.WriteLine("8. Update Animal");
                Console.WriteLine("9. View Recent Changes");
                Console.WriteLine("10. Exit");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddAnimal(zoo);
                        break;
                    case "2":
                        zoo.ListAnimals("Name");
                        break;
                    case "3":
                        zoo.ListAnimals("Color");
                        break;
                    case "4":
                        zoo.ListAnimals("Size");
                        break;
                    case "5":
                        zoo.ListAnimals("Weight");
                        break;
                    case "6":
                        RemoveAnimal(zoo);
                        break;
                    case "7":
                        SearchAnimal(zoo);
                        break;
                    case "8":
                        UpdateAnimal(zoo);
                        break;
                    case "9":
                        ViewRecentChanges(zoo);
                        break;
                    case "10":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddAnimal(Zoo zoo)
        {
            Console.Write("Enter animal name: ");
            string name = Console.ReadLine();

            Console.Write("Enter animal color: ");
            string color = Console.ReadLine();

            Console.Write("Enter animal size: ");
            string size = Console.ReadLine();

            Console.Write("Enter animal weight: ");
            string weight = Console.ReadLine();

            zoo.AddAnimal(new Animal(name, color, size, weight));
            Console.WriteLine("Animal added successfully.");
        }

        static void RemoveAnimal(Zoo zoo)
        {
            Console.Write("Enter the ID of the animal to remove: ");
            int id = int.Parse(Console.ReadLine());

            bool removed = zoo.RemoveAnimal(id);
            if (removed)
            {
                Console.WriteLine("Animal removed successfully.");
            }
            else
            {
                Console.WriteLine("Animal not found.");
            }
        }

        static void SearchAnimal(Zoo zoo)
        {
            Console.Write("Enter the name of the animal to search: ");
            string name = Console.ReadLine();

            List<Animal> animals = zoo.SearchAnimal(name);
            if (animals.Count > 0)
            {
                Console.WriteLine("Animals found:");
                foreach (var animal in animals)
                {
                    Console.WriteLine(animal);
                }
            }
            else
            {
                Console.WriteLine("Animal not found.");
            }
        }

        static void UpdateAnimal(Zoo zoo)
        {
            Console.Write("Enter the ID of the animal to update: ");
            int id = int.Parse(Console.ReadLine());

            Animal animal = zoo.SearchAnimalById(id);
            if (animal != null)
            {
                Console.Write("Enter new color (leave blank to keep current): ");
                string color = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(color)) animal.Color = color;

                Console.Write("Enter new size (leave blank to keep current): ");
                string size = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(size)) animal.Size = size;

                Console.Write("Enter new weight (leave blank to keep current): ");
                string weight = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(weight)) animal.Weight = weight;

                zoo.UpdateAnimal(animal);
                Console.WriteLine("Animal details updated successfully.");
            }
            else
            {
                Console.WriteLine("Animal not found.");
            }
        }

        static void ViewRecentChanges(Zoo zoo)
        {
            zoo.ViewRecentChanges();
        }
    }

    class Animal
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Weight { get; set; }

        public Animal(string name, string color, string size, string weight)
        {
            Name = name;
            Color = color;
            Size = size;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Color: {Color}, Size: {Size}, Weight: {Weight}";
        }
    }

    class Zoo
    {
        private LinkedList<Animal> animals;
        private AVLTree<Animal> animalTree;
        private Stack<string> recentChanges;
        private int nextId;
        private Dictionary<int, Animal> animalMap;

        public Zoo()
        {
            animals = new LinkedList<Animal>();
            animalTree = new AVLTree<Animal>((a1, a2) => a1.Name.CompareTo(a2.Name));
            recentChanges = new Stack<string>();
            nextId = 1;
            animalMap = new Dictionary<int, Animal>();
        }

        public void AddAnimal(Animal animal)
        {
            animal.ID = nextId++;
            animals.AddLast(animal);
            animalTree.Insert(animal);
            animalMap[animal.ID] = animal;
            recentChanges.Push($"Added: {animal}");
        }

        public bool RemoveAnimal(int id)
        {
            if (animalMap.TryGetValue(id, out var animal))
            {
                animals.Remove(animal);
                animalTree.Remove(animal);
                animalMap.Remove(id);
                recentChanges.Push($"Removed: {animal}");
                return true;
            }
            return false;
        }

        public List<Animal> SearchAnimal(string name)
        {
            var result = new List<Animal>();
            foreach (var animal in animals)
            {
                if (animal.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(animal);
                }
            }
            return result;
        }

        public Animal SearchAnimalById(int id)
        {
            animalMap.TryGetValue(id, out var animal);
            return animal;
        }

        public void UpdateAnimal(Animal animal)
        {
            recentChanges.Push($"Updated: {animal}");
        }

        public void ListAnimals(string sortBy)
        {
            var sortedAnimals = new List<Animal>(animals);

            switch (sortBy)
            {
                case "Name":
                    sortedAnimals.Sort((a1, a2) => a1.Name.CompareTo(a2.Name));
                    break;
                case "Color":
                    sortedAnimals.Sort((a1, a2) => a1.Color.CompareTo(a2.Color));
                    break;
                case "Size":
                    sortedAnimals.Sort((a1, a2) => a1.Size.CompareTo(a2.Size));
                    break;
                case "Weight":
                    sortedAnimals.Sort((a1, a2) => a1.Weight.CompareTo(a2.Weight));
                    break;
                default:
                    Console.WriteLine("Invalid sorting option.");
                    return;
            }

            if (sortedAnimals.Count == 0)
            {
                Console.WriteLine("No animals in the zoo.");
                return;
            }

            Console.WriteLine($"Animals sorted by {sortBy}:");
            foreach (var animal in sortedAnimals)
            {
                Console.WriteLine(animal);
            }
        }

        public void ViewRecentChanges()
        {
            if (recentChanges.Count == 0)
            {
                Console.WriteLine("No recent changes.");
                return;
            }

            Console.WriteLine("Recent changes:");
            foreach (var change in recentChanges)
            {
                Console.WriteLine(change);
            }
        }
    }

    // AVL Tree implementation (simplified)
    public class AVLTree<T>
    {
        private Node root;
        private readonly Comparison<T> comparison;

        public AVLTree(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        private class Node
        {
            public T Value;
            public Node Left;
            public Node Right;
            public int Height;

            public Node(T value)
            {
                Value = value;
                Height = 1;
            }
        }

        public void Insert(T value)
        {
            root = Insert(root, value);
        }

        private Node Insert(Node node, T value)
        {
            if (node == null) return new Node(value);

            int cmp = comparison(value, node.Value);
            if (cmp < 0)
                node.Left = Insert(node.Left, value);
            else if (cmp > 0)
                node.Right = Insert(node.Right, value);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            return Balance(node);
        }

        public void Remove(T value)
        {
            root = Remove(root, value);
        }

        private Node Remove(Node node, T value)
        {
            if (node == null) return null;

            int cmp = comparison(value, node.Value);
            if (cmp < 0)
                node.Left = Remove(node.Left, value);
            else if (cmp > 0)
                node.Right = Remove(node.Right, value);
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                Node temp = GetMin(node.Right);
                node.Value = temp.Value;
                node.Right = Remove(node.Right, temp.Value);
            }

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            return Balance(node);
        }

        private Node GetMin(Node node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        private int GetHeight(Node node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(Node node)
        {
            return GetHeight(node.Left) - GetHeight(node.Right);
        }

        private Node Balance(Node node)
        {
            int balance = GetBalance(node);

            if (balance > 1)
            {
                if (GetBalance(node.Left) < 0)
                    node.Left = RotateLeft(node.Left);

                node = RotateRight(node);
            }
            else if (balance < -1)
            {
                if (GetBalance(node.Right) > 0)
                    node.Right = RotateRight(node.Right);

                node = RotateLeft(node);
            }

            return node;
        }

        private Node RotateRight(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private Node RotateLeft(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }
    }
}
