public class Triangle {
    private int base;
    private int height;

    public Triangle() {
        this.base = 0;
        this.height = 0;
    }

    public Triangle(int base, int side) {
        this.base = base;
        this.height = side;
    }

    public int getBase() {
        return base;
    }

    public int getHeight() {
        return height;
    }

    public int getPerimeter() {
        return 2 * height + base;
    }

    public double getArea() {
        return 0.5 * base * getHeight();
    }


    /*maybe ill take this off*/
    public boolean isIsosceles() {
        return true;
    }

    public boolean isEquilateral() {
        return false;
    }

    public boolean isScalene() {
        return false;
    }

    public void print() {
        int repet, rest = 0;
        // Top of the triangle
        for (int i = 0; i < base / 2; i++) {
            System.out.print(" ");
        }
        System.out.println("*");

        // Middle of the triangle
        if (height > base) {
            repet = (height - 2) / ((base - 2) / 2);
            if ((height - 2) % ((base - 2) / 2) != 0) {
                rest = (height - 2) % ((base - 2) / 2);
            }
        } else {
            repet = (height - 2) / ((base - 2) / 2);
            if (((base - 2) / 2) % (height - 2) != 0) {
                rest = (height - 2) % ((base - 2) / 2);
            }
        }
        int middle_row = (base - 1) / 2 + 1;
        int num_stars = 3;
        for (int row = 0; row <= middle_row && num_stars < base; row++) {
            int num_spaces = (base - num_stars) / 2;
            if (row == 0) {
                repet += rest;
            }
            for (int i = 0; i < repet; i++) {
                for (int j = 0; j < num_spaces; j++) {
                    System.out.print(" ");
                }
                for (int j = 0; j < num_stars; j++) {
                    System.out.print("*");
                }
                System.out.println();
            }
            num_stars += 2;
            if (row == 0) {
                repet -= rest;
            }
        }

        // Bottom of the triangle
        for (int i = 0; i < base; i++) {
            System.out.print("*");
        }
        System.out.println();
    }
}