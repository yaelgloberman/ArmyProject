public class Rectangle {
    private int width;
    private int height;


    public Rectangle() {
        this.width = 0;
        this.height = 2;
    }
    public Rectangle(int width, int height) {
        this.width = width;
        this.height = height;
    }

    public int getWidth() {
        return width;
    }

    public int getHeight() {
        return height;
    }
    public void setWidth(int width) {
        this.width = width;
    }

    public void setHeight(int height) {
        this.height = height;
    }
    public int getArea() {
        return width * height;
    }

    public int getPerimeter() {
        return 2 * (width + height);
    }
    public boolean checkIfSquare()
    {
        if (height==width)
            return true;
        else
            return false;
    }
    public boolean checkIfGatherThan5()
    {
        if (Math.abs(height-width)>5)
            return true;
        else
            return false;
    }



}

