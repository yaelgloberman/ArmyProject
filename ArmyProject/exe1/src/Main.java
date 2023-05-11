import java.util.Scanner;
public class Main {
    public static void main(String[] args)
    {
        Scanner scanner = new Scanner(System.in);
        System.out.println("option 1\noption 2\noption 3");
        int option = 1;
        while (option!=3)
        {
            System.out.print("Enter your option: ");
            option = scanner.nextInt();
            if (option==1)
            {
                System.out.print("Enter the width:");
                int width=scanner.nextInt();
                System.out.print("Enter the height:");
                int height=scanner.nextInt();
                Rectangle rec = new Rectangle(width,height);

                if ( rec.checkIfSquare() || rec.checkIfGatherThan5())
                {
                    System.out.print("the area size is:");
                    System.out.print(rec.getArea());
                    System.out.println();

                }
                else
                {
                    System.out.print("the perimeter size is:");
                    System.out.print(rec.getPerimeter());
                    System.out.println();
                }
            }
            else
            {
                if (option==2)
                {
                    System.out.print("Enter the base:");
                    int base=scanner.nextInt();
                    System.out.print("Enter the height:");
                    int height=scanner.nextInt();
                    Triangle triangle = new Triangle(base,height);
                    System.out.print("option 1 - calculate triangle perimeter size");
                    System.out.print("option 1 - print triangle ");
                    System.out.print("Enter your option: ");
                    int optionT=scanner.nextInt();
                    if(optionT==1)
                    {
                        System.out.print("the perimeter size is:");
                        System.out.print(triangle.getPerimeter());
                        System.out.println();
                    }
                    if(optionT==2)
                    {
                        triangle.print();
                    }
                }

            }

        }

    }
}