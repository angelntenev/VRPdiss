import os

input_directory = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Split Routes"
output_directory = "C:\\Users\\Angel\\source\\repos\\VRPdiss\\DisplayNodes\\Customer nodes\\Routes for auction"

os.makedirs(output_directory, exist_ok=True)

file_count = len([name for name in os.listdir(input_directory) if os.path.isfile(os.path.join(input_directory, name))])

route_number = 1
for i in range(1, file_count+1, 10):
    output_filename = f"Route{route_number}.txt"
    output_filepath = os.path.join(output_directory, output_filename)

    with open(output_filepath, 'w') as output_file:
        for j in range(i, i + 10):
            input_filepath = os.path.join(input_directory, f"SeparatedRoute{j}.txt")

            with open(input_filepath, 'r') as input_file:
                content = input_file.read()
                output_file.write(content)

            if j < i + 9:
                output_file.write("NEXT\n")
        output_file.write("NEXT\n")
    
    route_number += 1
