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
                Console.WriteLine("9. Exit");
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
            Console.Write("Enter the name of the animal to remove: ");
            string name = Console.ReadLine();

            bool removed = zoo.RemoveAnimal(name);
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

            Animal animal = zoo.SearchAnimal(name);
            if (animal != null)
            {
                Console.WriteLine("Animal found: " + animal);
            }
            else
            {
                Console.WriteLine("Animal not found.");
            }
        }

        static void UpdateAnimal(Zoo zoo)
        {
            Console.Write("Enter the name of the animal to update: ");
            string name = Console.ReadLine();

            Animal animal = zoo.SearchAnimal(name);
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

                Console.WriteLine("Animal details updated successfully.");
            }
            else
            {
                Console.WriteLine("Animal not found.");
            }
        }
    }

    class Animal
    {
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
            return $"Name: {Name}, Color: {Color}, Size: {Size}, Weight: {Weight}";
        }
    }

    class Zoo
    {
        private LinkedList<Animal> animals;
        private AVLTree<Animal> animalTree;

        public Zoo()
        {
            animals = new LinkedList<Animal>();
            animalTree = new AVLTree<Animal>((a1, a2) => a1.Name.CompareTo(a2.Name));
        }

        public void AddAnimal(Animal animal)
        {
            animals.AddLast(animal);
            animalTree.Insert(animal);
        }

        public bool RemoveAnimal(string name)
        {
            var node = animals.First;
            while (node != null)
            {
                if (node.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    animals.Remove(node);
                    animalTree.Remove(node.Value);
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        public Animal SearchAnimal(string name)
        {
            foreach (var animal in animals)
            {
                if (animal.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return animal;
                }
            }
            return null;
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

                Node temp = node;
                node = Min(temp.Right);
                node.Right = RemoveMin(temp.Right);
                node.Left = temp.Left;
            }

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            return Balance(node);
        }

        private Node Balance(Node node)
        {
            int balance = GetBalance(node);
            if (balance > 1)
            {
                if (GetBalance(node.Left) < 0)
                    node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            if (balance < -1)
            {
                if (GetBalance(node.Right) > 0)
                    node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }
            return node;
        }

        private Node RotateLeft(Node node)
        {
            Node newRoot = node.Right;
            node.Right = newRoot.Left;
            newRoot.Left = node;
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            newRoot.Height = 1 + Math.Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right));
            return newRoot;
        }

        private Node RotateRight(Node node)
        {
            Node newRoot = node.Left;
            node.Left = newRoot.Right;
            newRoot.Right = node;
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            newRoot.Height = 1 + Math.Max(GetHeight(newRoot.Left), GetHeight(newRoot.Right));
            return newRoot;
        }

        private Node Min(Node node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        private Node RemoveMin(Node node)
        {
            if (node.Left == null) return node.Right;
            node.Left = RemoveMin(node.Left);
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
            return Balance(node);
        }

        private int GetHeight(Node node) => node?.Height ?? 0;
        private int GetBalance(Node node) => GetHeight(node.Left) - GetHeight(node.Right);
    }
}
-