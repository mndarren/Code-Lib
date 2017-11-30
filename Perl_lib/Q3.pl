#!/usr/bin/perl -w
use strict;
use Modern::Perl; # include feature say
use Data::Dumper;
#use Mail::RFC822::Address qw(valid);# for email check

# @void function: print menu
sub menu {
   say "-" x 40;
   say "1. Search a person by name or phone #;";
   say "2. Add a new person in address book;";
   say "3. Delete a person by name or phone #;";
   say "4. Print all info of the address book";
   say "5. Save address book as a file;";
   say "6. Exit.";
   print "\nPlease choose a number [1-6]: ";
}
# @params: (reference of address book) and (name or phone #)
# @void function: search a person in address book
sub search_person {
   my $ref_hash = shift;
   my @result_array;
   print "Please enter a person name or phone (###-###-####): ";
   my $input = <>;
   $input =~ s/^\s*|\s*$//g;# trim both ends
   return if (length($input) == 0); # check empty input
   # got it from online, need more test
   if(($input !~ m/^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$/g) && ($input !~ m/^[0-9]{10}$/)) {
      say "[$input] is not a person name or a phone #!";
      return 0;
   }
   foreach my $key (keys %{$ref_hash}) {
      foreach my $key_in (keys %{$ref_hash->{$key}}) {
         push @result_array, $ref_hash->{$key}->{$key_in} if ($key_in =~ m/($input)+/);
      }
   }
   if (scalar @result_array) {
      foreach my $var (@result_array){
         &print_hash($var);
      }
   } else {
      say "Couldn't find [$input]!";
   }
}
# @params: a ref of hash
# @void function: add a person info into the address book
sub add_person {
   my $ref_hash = shift;
   print "Please input the person name (Firstname Lastname): ";
   my $name = <>; 
   $name =~ s/^\s*|\s*$//g;# trim both ends
   if (is_person_exist($ref_hash,$name)) {
      say "The person [$name] has already been in the book!\n";
      return 0;
   }
   if($name !~ m/^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$/g) {
      say "[$name] is not a person name!";
      return 0;
   }
   # check if the name format is Lastname,Firstname
   my $index_coma = index($name, ',');
   if( $index_coma != -1) {
      my $firstname = substr($name,$index_coma+1,length($name));
      my $lastname = substr($name,0,$index_coma);
      $name = $firstname . ' ' . $lastname;
   }

   print "Please input the person phone(###-###-####): ";
   my $phone = <>; 
   $phone =~ s/^\s*|\s*$//g;
   $phone =~ s/-//g;
   if ($phone !~ m/^[0-9]{10}$/) { # check the # of digits
      say "[$phone] is not a phone #!";
      return 0;
   }
   substr($phone, 3, 0) = '-';
   substr($phone, 7, 0) = '-';
   if (is_person_exist($ref_hash,$phone)) { # check if it exists
      say "The person [$phone] has already been in the book!\n";
      return 0;
   }

   print "Please input the person address: ";
   my $address = <>; 
   $address =~ s/^\s*|\s*$//g;
   # (($address !~ /.*?\s+<?(\S+?)>?\s+F=/) || (!valid($1))) #for email check

   if ($address !~ /\s+(\d{2,5}\s+)(?![a|p]m\b)(([a-zA-Z|\s+]{1,5}){1,2})?
            ([\s|\,|.]+)?(([a-zA-Z|\s+]{1,30}){1,4})(court|ct|street|st|drive|dr|lane|ln|road|rd|blvd)
            ([\s|\,|.|\;]+)?(([a-zA-Z|\s+]{1,30}){1,2})([\s|\,|.]+)?\b(AK|AL|AR|AZ|CA|CO|CT|DC|DE|FL|GA
            |GU|HI|IA|ID|IL|IN|KS|KY|LA|MA|MD|ME|MI|MN|MO|MS|MT|NC|ND|NE|NH|NJ|NM|NV|NY|OH|OK|OR|PA|RI|
            SC|SD|TN|TX|UT|VA|VI|VT|WA|WI|WV|WY)([\s|\,|.]+)?(\s+\d{5})?([\s|\,|.]+)/ix) {
      say "[$address] is not an address!";
   }
   my $temp_ref_hash->{"name"} = $name;
   $temp_ref_hash->{"phone"} = $phone;
   $temp_ref_hash->{"address"} = $address;
   &print_hash($temp_ref_hash);

   $ref_hash->{"by_phone"}->{$phone} = $temp_ref_hash;
   $ref_hash->{"by_name"}->{$name} = $temp_ref_hash;
   say "The new person is successfully added!\n";
}

# @params: (reference hash of address book) and (a name or a phone #)
# @void function: delete a person from address book
sub delete_person {
   my $ref_hash = shift;
   print "Please input the person to be deleted name or phone(###-###-####): ";
   my $input = <>;
   $input =~ s/^\s*|\s*$//g;
   my $is_exist = &is_person_exist($ref_hash, $input);
   if (!$is_exist) {
      say "Error deleting! [$input] does not exist!";
      return 0;
   }
   delete $ref_hash->{by_name}->{$is_exist->{name}};
   delete $ref_hash->{by_phone}->{$is_exist->{phone}};
   say "[$input] has successfully deleted!";
}
# @param: a hash reference
sub print_all {
   my $ref_hash = shift;
   foreach my $key (keys %{$ref_hash}) {
      &print_hash($ref_hash->{$key});
   }
}
# @void function: save all data into a file
# @param: a hash ref
sub save_file {
   my $ref_hash = shift;
   my $file = "address_book$$.txt";
   my $fh_out;
   warn "Couldn't open [$file]: $!" if !open $fh_out, '>', $file;
   
   if (write_file($ref_hash, $fh_out)) {
      say "Address book has saved in [$file] successfully!";
   } else {
      say "Failed to save file!";
   }
}

# @function: read data from file into a hash ref
# @params: a hash ref and a file handle
sub read_book {
   my $ref_hash = shift;
   my $fh_in = shift;
   my ($ref_hash_name, $ref_hash_phone);
   my @keys_array = qw/name phone address/;
   foreach my $line (<$fh_in>) {
     $line =~ s/\"\s*\"/|/g;
     $line =~ s/\"//g;
     chomp ($line);
     my @array = split(/\|/,$line);
     my $temp_ref_hash;
     for (my $i = 0; $i < @array; $i++) {
        $temp_ref_hash->{$keys_array[$i]} = $array[$i];
        if($i == 0) {
            $ref_hash_name->{$array[$i]} = $temp_ref_hash;
        } elsif ($i == 1) {
            $ref_hash_phone->{$array[$i]} = $temp_ref_hash;
        }
     }
   }
   #say Dumper \$ref_hash_phone;
   #say Dumper \$ref_hash_name;
   $ref_hash->{by_name} = $ref_hash_name;
   $ref_hash->{by_phone} = $ref_hash_phone;
   #say Dumper \$ref_hash; #Dumper doesn't work well sometimes
   return $ref_hash;
}
# helper function: print out all hash data
# @input: the hash reference (only one layer ref)
sub print_hash {
   my $hash = shift;
   print "\n{\n";
   while (my ($key, $value) = each %{$hash}) {
      say "   $key => $value";
   }
   print "}\n";
}
# @helper function: return a hash ref if found the person or return 0 if not
# @params: a hash ref and a input data
sub is_person_exist {
   my $ref_hash = shift;
   my $input = shift;
   foreach my $key (keys %{$ref_hash}) {
      foreach my $key_in (keys %{$ref_hash->{$key}}) {
         return $ref_hash->{$key}->{$key_in} if ($key_in eq $input);
      }
   }
   return 0;
}
# helper function for save_file()
# @params: a hash ref and a file handle
# @return true
sub write_file {
   my $ref_hash = shift;
   my $fh_out = shift;
   select $fh_out;
   $| = 1; # avoid $fh_out sitting in the buffer
   my @keys_array = qw/name phone address/;
   while (my ($key, $value_ref) = each %{$ref_hash->{by_phone}}) {
      my $i = 0;
      foreach my $key_in (keys %{$value_ref}) {
         print '"' . $value_ref->{$keys_array[$i++]} . '" ';
      }
      print "\n";
   }
   select STDOUT;
   $| = 0;
   return 1;
}

sub main {
   my ($fh_in, $ref_hash);
   my $file_name = 'phonebook_exercise.txt';
   die "Cannot open the file: $file_name for read: $!" if (!open $fh_in, '<', $file_name);
   #Read the whole data from address book into $ref_hash, but in the real world
   #it's not good idea since it will eat a lot of memory if the data is too big.
   # We'll use DB to solve it.
   $ref_hash = &read_book($ref_hash, $fh_in);
   &menu();
   while(my $choice = <>) {
      if ($choice !~ m{[1-6]{1}}) {
         say "Invalid input!";
      } else {
         $choice == 1 ? &search_person($ref_hash)          :
         $choice == 2 ? &add_person($ref_hash)             :
         $choice == 3 ? &delete_person($ref_hash)          :
         $choice == 4 ? &print_all($ref_hash->{by_phone})  :
         $choice == 5 ? &save_file($ref_hash)              :
                   exit();
      }
      &menu();
   }
}
main();
