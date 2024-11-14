using System;
using System.Collections.Generic;
using System.IO;

/* I have MacOS and there is not opportunity
 for me to make UI by windows forms or wpf
 so I did it using terminal because
 I don't see another option*/



class Program
{
    static void Main(string[] args)
    {
        Shop shop = new Shop();
        shop.AddStyle(new Style(1, "Jackets", "Straight", "Black", "Wool", "Brand A", 500));
        shop.AddStyle(new Style(2, "Tux", "Loose", "Black", "Synthetic", "Brand B", 100));
        shop.AddStyle(new Style(3, "Jeans", "Tight", "Blue", "Synthetic", "Brand C", 200));

        shop.AddSize(1, "S", 0);
        shop.AddSize(1, "M", 5);
        shop.AddSize(1, "L", 10);
        shop.AddSize(2, "S", 0);
        shop.AddSize(2, "M", 5);
        shop.AddSize(2, "L", 10);
        shop.AddSize(3, "S", 0);
        shop.AddSize(3, "M", 5);
        shop.AddSize(3, "L", 10);

        
        StyleWithSizePrice(shop);

        Console.WriteLine("\nSorting styles by type: ");
        shop.SortStylesByType();
        StyleWithSizePrice(shop);

        Console.WriteLine("\nSorting styles by color: ");
        shop.SortStylesByColor();
        StyleWithSizePrice(shop);

        Console.WriteLine("\nSorting styles by price: ");
        shop.SortStylesByPrice();
        StyleWithSizePrice(shop);

        // Deletev style, size and print the output
        shop.DeleteStyle(3);
        shop.DeleteSize(1, "L");
        Console.WriteLine($"Styles with sizes after being deleted:");
        StyleWithSizePrice(shop);


        // Modifying style, size and print the output
        Style modifyStyle = new Style(2, "Tuxedo","Slim", "Black", "Polyester", "Brand V", 120);
        shop.ModifyStyle(modifyStyle);
        shop.ModifySize(1, "M", 60);
        Console.WriteLine($"Style with sizes after being modified: \n{modifyStyle}");

        StyleWithSizePrice(shop);


        string filePath = "shopdata.txt";
        shop.SaveToFile(filePath);
       

        Console.ReadLine();
    }

    //Count final price and write information about each product 
    static void StyleWithSizePrice(Shop shop)
    {
        foreach (var style in shop.GetStyles())
        {
            Console.WriteLine("____________________________________________________________________________________________\n\n" + style);
            foreach (var size in style.Sizes)
            {
                decimal finalPrice = shop.CalculateFinalPrice(style.Id, size.SizeOfClothes);
                Console.WriteLine($"\nSize:\t Final Price:\n{size.SizeOfClothes}\t {finalPrice}");
            }
        }
    }
}

//Specify Cut class
public class Cut
{
    public static readonly Cut Straight = new Cut("Straight");
    public static readonly Cut Loose = new Cut("Loose");
    public static readonly Cut Tight = new Cut("Tight");

    public string NameOfCut { get; }

    private Cut(string nameOfcut)
    {
        NameOfCut = nameOfcut;
    }

    public override string ToString()
    {
        return NameOfCut;
    }
}

//Specify Fabric class
public class Fabric
{
    public static readonly Fabric Cotton = new Fabric("Cotton");
    public static readonly Fabric Wool = new Fabric("Wool");
    public static readonly Fabric Synthetic = new Fabric("Syntheric");

    public string NameOfFabric { get; }
    private Fabric(string nameOfFabric)
    {
        NameOfFabric = nameOfFabric;

    }

    public override string ToString()
    {
        return NameOfFabric;
    }
}

//Specify Size class
public class Size
{
    public string SizeOfClothes { get; set; }
    public decimal FinalPrice { get; set; }

    public Size(string size, decimal fprice)
    {
        SizeOfClothes = size;
        FinalPrice = fprice;
    }

    public override string ToString()
    {
        return $"{SizeOfClothes}, {FinalPrice}";
    }

}

//Specify Style class
public class Style
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Cut { get; set; }
    public string Color { get; set; }
    public string Fabric { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public List<Size> Sizes { get; set; }


    public Style(int id, string type, string cut, string color, string fabric, string brand, decimal price)
    {
        Id = id;
        Type = type;
        Cut = cut;
        Color = color;
        Fabric = fabric;
        Brand = brand;
        Price = price;
        Sizes = new List<Size>();
        
    }

    public override string ToString()
    {
        return $"ID: {Id}, Type: {Type}, Cut: {Cut}, Color: {Color}, Fabric: {Fabric}, Brand: {Brand}, Price: {Price}";
    }
}

public class Shop
{
    //Create style list 
    public List<Style> styles;

    public Shop()
    {
        styles = new List<Style>();
    }

    //Calculae final price

    public decimal CalculateFinalPrice(int styleId, string sizeOfClothes)
    {
        foreach (var style in styles)
        {
            if (style.Id == styleId)
            {
                foreach (var size in style.Sizes)
                {
                    if (size.SizeOfClothes == sizeOfClothes)
                    {
                        return style.Price + size.FinalPrice;
                    }
                }
                Console.WriteLine($"Size '{sizeOfClothes}' not found in style ID {styleId}.");
                return -1; // Return -1 to indicate size not found
            }
        }
        Console.WriteLine($"Style ID {styleId} not found.");
        return -1; // Return -1 to indicate style not found
    }

    public List<Style> Select(string type, string color, string size)
    {
        List<Style> selectedStyles = new List<Style>();

        foreach (var style in styles)
        {
            if (style.Type == type && style.Color == color)
            {
                foreach (var sizeInfo in style.Sizes)
                {
                    if (sizeInfo.SizeOfClothes == size)
                    {
                        selectedStyles.Add(style);
                        break;
                    }
                }
            }
        }

        return selectedStyles;
    }

    //Add function for style

    public void AddStyle(Style style)
    {

        styles.Add(style);

    }

    //Delete function for style
    public void DeleteStyle(int id)
    {
        Style styleToRemove = null;
        foreach (var style in styles)
        {
            if (style.Id == id)
            {
                styleToRemove = style;
                break;
            }
        }

        if (styleToRemove != null)
        {
            styles.Remove(styleToRemove);
            Console.WriteLine($"\nStyle with ID {id} deleted.");
        }
        else
        {
            Console.WriteLine($"\nStyle with ID {id} not found.");
        }
    }

    //Modify function for style
    public void ModifyStyle(Style modifyStyle)
    {
        bool found = false;
        foreach (var style in styles)
        {
            if (style.Id == modifyStyle.Id)
            {
                style.Type = modifyStyle.Type;
                style.Cut = modifyStyle.Cut;
                style.Color = modifyStyle.Color;
                style.Fabric = modifyStyle.Fabric;
                style.Brand = modifyStyle.Brand;
                style.Price = modifyStyle.Price;
                found = true;
                break;
            }
        }
        if (found)
        {
            Console.WriteLine($"\nID style {modifyStyle.Id} was modifies successfully");
        }
        else
        {
            Console.WriteLine($"\nId style {modifyStyle.Id} not found");
        }

    }

    //Add for size
    public void AddSize(int styleId, string sizeName, decimal price)
    {
        foreach (var style in styles)
        {
            if (style.Id == styleId)
            {
                foreach (var size in style.Sizes)
                {
                    if (size.SizeOfClothes == sizeName)
                    {
                        Console.WriteLine($"\nSize {sizeName} already exists in style ID {styleId}");
                        return;
                    }
                }
                // If the size doesn't exist, add it to the style's Sizes list
                style.Sizes.Add(new Size(sizeName, price));
                return;
            }
        }
        Console.WriteLine($"\nStyle ID {styleId} not found");
    }

    //Delete for size
    public void DeleteSize(int styleId, string sizeOfClothes)
    {
        foreach (var style in styles)
        {
            if (style.Id == styleId)
            {
                foreach (var size in style.Sizes)
                {
                    if (size.SizeOfClothes == sizeOfClothes)
                    {
                        style.Sizes.Remove(size);
                        Console.WriteLine($"Size {sizeOfClothes} was deleted from style ID {styleId}");
                        return;
                    }
                }
                Console.WriteLine($"Size is not found in style ID {styleId}");
                return;
            }
        }
        Console.WriteLine("Style is not found");
    }

    //Modify for size
    public void ModifySize(int styleId, string sizeOfClothes, decimal newPrice)
    {
        foreach (var style in styles)
        {
            if (style.Id == styleId)
            {
                foreach (var size in style.Sizes)
                {
                    if (size.SizeOfClothes == sizeOfClothes)
                    {
                        size.FinalPrice = newPrice;
                        Console.WriteLine($"Price of size {sizeOfClothes} in style id {styleId} after being modified is {size.FinalPrice}");
                        return;
                    }
                }
                Console.WriteLine($"Size is nout found in style id{styleId}");
                return;
            }
        }
        Console.WriteLine("Is not found.");
    }


    public List<Style> GetStyles()
    {
        return styles;
    }

    //Save file as text

    public void SaveToFile(string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Style style in styles)
                {
                    string data = $"{style.Id} " +
                        $"           {style.Type} " +
                        $"           {style.Cut} " +
                        $"           {style.Color} " +
                        $"           {style.Fabric} " +
                        $"           {style.Brand} " +
                        $"           {style.Price}";
                    writer.WriteLine(data);

                    if (style.Sizes != null)
                    {
                        foreach (Size size in style.Sizes)
                        {
                            writer.WriteLine($"{size.SizeOfClothes},{size.FinalPrice}");
                        }
                    }
                    writer.WriteLine(); // Empty line to separate styles
                }
            }
            Console.WriteLine("Data has been saved to file successfully.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An I/O error occurred while saving data: {ex.Message}");
        }
    }

    // Method to load the data from a text file
    public void LoadFromFile(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                List<Style> styles = new List<Style>();

                Style style = null;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) // Empty line indicates the end of a style's data
                    {
                        if (style != null)
                        {
                            // Add the style to the styles list
                            styles[styles.Count - 1] = style;
                            style = null; // Reset style for the next iteration
                        }
                    }
                    else
                    {
                        if (style == null)
                        {
                            string[] styleData = line.Split(',');
                            int id = int.Parse(styleData[0]);
                            string type = styleData[1];
                            string cut = styleData[2];
                            string color = styleData[3];
                            string fabric = styleData[4];
                            string brand = styleData[5];
                            decimal price = decimal.Parse(styleData[6]);
                            style = new Style(id, type, cut, color, fabric, brand, price);
                        }
                        else
                        {
                            string[] sizeData = line.Split(',');
                            string sizeName = sizeData[0];
                            decimal priceSurcharge = decimal.Parse(sizeData[1]);
                            style.Sizes.Add(new Size(sizeName, priceSurcharge));
                        }
                    }
                }
                // Add the last style if it hasn't been added yet (in case the file doesn't end with an empty line)
                if (style != null)
                {
                    styles[styles.Count - 1] = style;
                }
            }
            Console.WriteLine("Data has been loaded from file successfully.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An I/O error occurred while loading data: {ex.Message}");
        }
    }

    public void SortStylesByType()
    {
        for (int i = 0; i < styles.Count - 1; i++)
        {
            for (int j = 0; j < styles.Count - i - 1; j++)
            {
                if (string.Compare(styles[j].Type, styles[j + 1].Type) > 0)
                {
                    Style temp = styles[j];
                    styles[j] = styles[j + 1];
                    styles[j + 1] = temp;
                }
            }
        }
    }

    public void SortStylesByColor()
    {
        for (int i = 0; i < styles.Count - i; i++)
        {
            for (int j = 0; j < styles.Count - i - 1; j++)
            {
                if (string.Compare(styles[j].Color, styles[j + 1].Color) > 0)
                {
                    Style temp = styles[j];
                    styles[j] = styles[j + 1];
                    styles[j + 1] = temp;
                }

            }
        }
    }

    public void SortStylesByPrice()
    {
        for (int i = 0; i < styles.Count - 1; i++)
        {
            for (int j = 0; j < styles.Count - i - 1; j++)
            {
                if (styles[j].Price > styles[j + 1].Price)
                {
                    Style temp = styles[j];
                    styles[j] = styles[j + 1];
                    styles[j + 1] = temp;
                }
            }
        }
    }
}

