import os
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import tkinter as tk

# get a list of all files in the directory
directory = r'C:\Users\Angel\source\repos\VRPdiss\DisplayNodes\Customer nodes\Solution to Routes'
files = [os.path.join(directory, f) for f in os.listdir(directory) if f.endswith('.txt')]

# create a function to handle button clicks
def on_button_click(direction):
    global i
    if direction == 'forward':
        i = (i + 1) % len(files)
    elif direction == 'backward':
        i = (i - 1) % len(files)
    plot_file(files[i])

# create a function to plot a file
def plot_file(file):
    with open(file) as f:
        lines = f.readlines()
    points = [tuple(map(int, line.strip().split(','))) for line in lines]
    x, y = zip(*points)
    plt.clf()  # clear the plot
    plt.scatter(x, y)
    colors = ['r', 'g', 'b', 'c', 'm', 'y', 'k']
    for j in range(len(points) - 1):
        x1, y1 = points[j]
        x2, y2 = points[j+1]
        label = str(j+1)
        color = colors[j % len(colors)]
        plt.arrow(x1, y1, x2-x1, y2-y1, length_includes_head=True, head_width=20, color=color)
        plt.annotate(label, xy=((x1+x2)/2, (y1+y2)/2), xytext=(5, 5), textcoords='offset points', color=color)
    plt.title(os.path.basename(file))
    canvas.draw()

# create the Tkinter window
root = tk.Tk()
root.geometry('800x600')

# create the Matplotlib figure and canvas
fig = plt.figure()
canvas = FigureCanvasTkAgg(fig, master=root)
canvas.get_tk_widget().pack(side=tk.TOP, fill=tk.BOTH, expand=1)

# create the backward and forward buttons
button_frame = tk.Frame(root)
button_frame.pack(side=tk.BOTTOM)
backward_button = tk.Button(button_frame, text='Backward', command=lambda: on_button_click('backward'))
backward_button.pack(side=tk.LEFT)
forward_button = tk.Button(button_frame, text='Forward', command=lambda: on_button_click('forward'))
forward_button.pack(side=tk.LEFT)

# plot the first file
i = 0
plot_file(files[i])

# start the Tkinter main loop
root.mainloop()
